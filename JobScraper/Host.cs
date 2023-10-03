using JobScraper.ViewModel.EventArgs;
using JobScraper.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using JobScraper.View.Pages;
using JobScraper.View.Components;
using JobScraper.Model.Processing;
using JobScraper.Model.Scraping;
using JobScraper.Model.Scraping.Events;
using JobScraper.Model.Data;

namespace JobScraper
{
    public partial class Host : Form
    {
        public Host()
        {
            InitializeComponent();

            //var database = new Database();
            //
            //var jobIndexScraper = new JobIndexScraper(database);
            //
            //jobIndexScraper.OnInitStarted += (s, e) => {
            //    MessageBox.Show("Started init!");
            //};
            //
            //jobIndexScraper.OnInitDone += (s, e) => {
            //    MessageBox.Show("Init done!");
            //};
            //
            //jobIndexScraper.OnAdScrapingStarted += (s, e) =>
            //{
            //    MessageBox.Show("Scraping started!");
            //};
            //
            //jobIndexScraper.OnAdScrapingDone += (s, e) => {
            //    MessageBox.Show("Scraping done!");
            //};
            //
            //jobIndexScraper.OnAdFetchingStarted += (s, e) => {
            //    AdFetchingStartedEvent se = (AdFetchingStartedEvent)e;
            //    MessageBox.Show("Fetching ads started! : " + se.totalAds);
            //};
            //
            //jobIndexScraper.OnAdFetchingProgress += (s, e) => {
            //    AdFetchingProgressEvent pe = (AdFetchingProgressEvent)e;
            //    MessageBox.Show("Fetched ad : " + pe.fetchedAd.Title);
            //};
            //
            //
            //database.SetAdFetchedCallback(jobIndexScraper);
            //
            //var processor = new SimpleProcessor(database);
            //processor.SetAdFetchedCallback(jobIndexScraper);
            //
            //jobIndexScraper.Init();
            //jobIndexScraper.Start();

            MainViewModel viewModel = new MainViewModel();

            EventCallback<KeywordArg> AddKeywordCallback = new EventCallback<KeywordArg>(null, (KeywordArg args) =>
            {
                viewModel.TriggerOnKeywordAdded(args.Keyword);
            });

            EventCallback<KeywordArg> RemoveKeywordCallback = new EventCallback<KeywordArg>(null, (KeywordArg args) =>
            {
                viewModel.TriggerOnKeywordRemoved(args.Keyword);
            });

            FilterParams filterParams = new FilterParams
            {
                AddKeywordCallback = AddKeywordCallback,
                RemoveKeywordCallback = RemoveKeywordCallback
            };
            
            var services = new ServiceCollection();
            services.AddWindowsFormsBlazorWebView();
            blazorWebView.HostPage = "wwwroot\\index.html";
            blazorWebView.Services = services.BuildServiceProvider();
            blazorWebView.RootComponents.Add<Main>("#app",
                new Dictionary<string, object?>
                {
                    { "ViewModel", viewModel }
                });
        }
    }
}