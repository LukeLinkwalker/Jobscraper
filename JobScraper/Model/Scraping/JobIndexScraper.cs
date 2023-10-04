using JobScraper.Model.Data;
using JobScraper.Model.Scraping.Events;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using JobScraper.Utils;

namespace JobScraper.Model.Scraping
{
    public class JobIndexScraper : IScraper
    {
        private const string DEBUG_FILTER = "JOBINDEXSCRAPER";

        public event EventHandler OnInitStarted;
        public event EventHandler OnInitDone;
        public event EventHandler OnAdScrapingStarted;
        public event EventHandler OnAdScrapingDone;
        public event EventHandler OnAdFetchingStarted;
        public event EventHandler OnAdFetchingDone;
        public event EventHandler OnAdFetchingProgress;
        public bool IsRunning;

        private BrowserFetcher _browserFetcher;
        private IBrowser _browser;
        private DateTime _lastJobIndexVisit;
        private DateTime _lastScraping;
        private System.Timers.Timer _scrapeTimer;
        private bool _initialized;
        private Database _database;

        public JobIndexScraper(Database database)
        {
            _lastJobIndexVisit = DateTime.MinValue;
            _lastScraping = DateTime.MinValue;
            _initialized = false;
            _database = database;
            IsRunning = false;
        }

        public void Init()
        {
            Initialize();
        }

        public void Start()
        {
            _scrapeTimer = new System.Timers.Timer(1000);
            _scrapeTimer.Elapsed += OnTimedScrape;
            _scrapeTimer.AutoReset = true;
            _scrapeTimer.Enabled = true;

            IsRunning = true;
        }

        public void Stop()
        {
            _scrapeTimer.Stop();

            IsRunning = false;
        }


        private void OnTimedScrape(object source, ElapsedEventArgs e)
        {
            if (_initialized == true)
            {
                DateTime currentTime = DateTime.Now;

                if (currentTime.Subtract(_lastScraping).TotalMinutes > 60 || _lastScraping == DateTime.MinValue)
                {
                    _lastScraping = DateTime.Now;
                    Scrape();
                }
            }
        }

        private async void Scrape()
        {
            OnAdScrapingStarted?.Invoke(this, EventArgs.Empty);

            // Get number of pages to scrape
            int numberOfPagesToScrape = await FindNumberOfPages(false);
            Debug.WriteLine(DEBUG_FILTER + " - Number of pages to scrape : " + numberOfPagesToScrape);

            // Get URLs of ads to scrape
            List<Ad> listOfAdsToScrape = new List<Ad>();
            for (int i = 1; i <= numberOfPagesToScrape; i += 1)
            {
                List<Ad> listOfAds = await FetchAdListings(i);
                listOfAdsToScrape.AddRange(listOfAds);

                // Throttling visits to jobindex.dk
                await Task.Delay(1000);
            }
            Debug.WriteLine(DEBUG_FILTER + " - Number of ads to scrape : " + listOfAdsToScrape.Count);

            // Filter out known ads from ads to be scraped
            for(int i = listOfAdsToScrape.Count - 1; i >= 0; i -= 1)
            {
                if (_database.ContainsAd(listOfAdsToScrape[i].URL))
                {
                    Debug.WriteLine(DEBUG_FILTER + " - Removed ad from scraping because it is known");
                    listOfAdsToScrape.Remove(listOfAdsToScrape[i]);
                }
            }

            // Scrape each URL for ad content
            OnAdFetchingStarted?.Invoke(this, new AdFetchingStartedEvent(listOfAdsToScrape.Count));
            for (int i = 0; i < listOfAdsToScrape.Count; i += 1)
            {
                Ad currentAd = listOfAdsToScrape[i];
                string content = await FetchAdContent(currentAd.URL);
                currentAd.Content = content;

                OnAdFetchingProgress?.Invoke(this, new AdFetchingProgressEvent(i, currentAd));
            }
            OnAdFetchingDone?.Invoke(this, EventArgs.Empty);

            OnAdScrapingDone?.Invoke(this, EventArgs.Empty);
        }

        private async void Initialize()
        {
            OnInitStarted?.Invoke(this, EventArgs.Empty);

            _browserFetcher = new BrowserFetcher();
            await _browserFetcher.DownloadAsync();
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            _initialized = true;

            OnInitDone?.Invoke(this, EventArgs.Empty);
        }

        private async Task<int> FindNumberOfPages(bool recentOnly)
        {
            int result = 1;

            using (IPage page = await _browser.NewPageAsync())
            {
                await page.GoToAsync("https://www.jobindex.dk/jobsoegning/it/systemudvikling?jobage=7");

                IElementHandle pagination = await page.QuerySelectorAsync(".pagination");
                if (pagination != null)
                {
                    List<string> pageItemTexts = await page.QueryAllElementsAndProperties(".page-item", "innerText");
                    result = int.Parse(pageItemTexts[pageItemTexts.Count - 2]);
                }
            }

            return result;
        }

        private async Task<List<Ad>> FetchAdListings(int pageNumber)
        {
            List<Ad> result = new List<Ad>();

            using (IPage page = await _browser.NewPageAsync())
            {
                await page.GoToAsync("https://www.jobindex.dk/jobsoegning/it/systemudvikling?jobage=7&page=" + pageNumber);

                IElementHandle[] JobListings = await page.QuerySelectorAllAsync(".jobsearch-result");

                foreach (IElementHandle handle in JobListings)
                {
                    string titleString = await handle.QueryElementAndProperty("h4", "innerText");
                    IElementHandle titleElement = await handle.QuerySelectorAsync("h4");
                    string urlString = await titleElement.QueryElementAndProperty("a", "href");

                    result.Add(new Ad()
                    {
                        Title = titleString,
                        URL = urlString,
                        Timestamp = DateTime.Now.ToString()
                    });
                }
            }

            return result;
        }

        async Task<string> FetchAdContent(string URL)
        {
            string result = string.Empty;

            try
            {
                using (IPage page = await _browser.NewPageAsync())
                {
                    await page.GoToAsync(URL);
                    result = await page.QueryElementAndProperty("body", "innerText");

                    // Throttling visits to jobindex.dk
                    if (URL.ToLower().Contains("jobindex.dk"))
                    {
                        DateTime currentTime = DateTime.Now;

                        if (currentTime.Subtract(_lastJobIndexVisit).TotalSeconds < 1)
                        {
                            await Task.Delay(1000);
                        }

                        _lastJobIndexVisit = DateTime.Now;
                    }
                }
            }
            catch (NavigationException ex)
            {
                // Failed to fetch ad sometimes due to timing out.
                // Emit event ?
            }

            return result;
        }
    }
}