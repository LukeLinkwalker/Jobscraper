using JobScraper.Model.Data;
using JobScraper.Model.Processing.Events;
using JobScraper.Model.Scraping;
using JobScraper.Model.Scraping.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.Model.Processing
{
    public class SimpleProcessor : IProcessor
    {
        private const string DEBUG_FILTER = "SIMPLEPROCESSOR";

        public event EventHandler OnAdsProcessed;

        private Database _database;
        private List<Ad> _ads;
        private List<string> _keywords;

        public SimpleProcessor(Database database)
        {
            _database = database; 
            _keywords = new List<string>();
            _ads = _database.GetAds();

            ProcessAds();
        }

        public void SetAdFetchedCallback(IScraper scraper)
        {
            scraper.OnAdFetchingProgress += (s, e) =>
            {
                AdFetchingProgressEvent pe = (AdFetchingProgressEvent)e;
                _ads.Add(pe.fetchedAd);
                ProcessAds();
            };
        }

        public void AddKeyword(string keyword)
        {
            _keywords.Add(keyword);
            ProcessAds();
        }

        public void RemoveKeyword(string keyword)
        {
            _keywords.Remove(keyword);
            ProcessAds();
        }

        public void ForceProcess()
        {
            ProcessAds();
        }

        public void ProcessAds()
        {
            List<Ad> processedAds = _database.GetAds();

            if(_keywords.Count > 0) 
            {
                foreach (Ad ad in processedAds)
                {
                    string txt = ad.Content.ToLower();
                    ad.Keywords.Clear();

                    foreach (string keyword in _keywords)
                    {
                        if (txt.Contains(keyword.ToLower()))
                        {
                            ad.Keywords.Add(keyword);
                        }
                    }
                }

                for(int i = processedAds.Count - 1; i >= 0; i -= 1)
                {
                    if (processedAds[i].Keywords.Count == 0)
                    {
                        processedAds.RemoveAt(i);
                    }
                }
            }

            OnAdsProcessed?.Invoke(this, new AdsProcessedEvent(processedAds));
        }
    }
}
