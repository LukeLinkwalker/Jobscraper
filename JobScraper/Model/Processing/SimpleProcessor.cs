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
        public event EventHandler OnAdProcessed;
        public event EventHandler OnAdsProcessed;

        private Database _database;
        private List<Ad> _ads;
        private List<string> _keywords;

        public SimpleProcessor(Database database)
        {
            _database = database; 
            _keywords = new List<string>();
            ProcessAds();
        }

        public void SetAdFetchedCallback(IScraper scraper)
        {
            scraper.OnAdFetchingProgress += (s, e) =>
            {
                AdFetchingProgressEvent pe = (AdFetchingProgressEvent)e;
                ProcessAd(pe.fetchedAd);
            };
        }

        public void SetKeywordsChangedCallback()
        {
            // Use ProcessAds
        }

        private void ProcessAds()
        {
            foreach (Ad ad in _database.GetAds())
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

            OnAdsProcessed?.Invoke(this, new AdsProcessedEvent(_ads));
        }

        private void ProcessAd(Ad ad)
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

            OnAdProcessed?.Invoke(this, new AdProcessedEvent(ad));
        }
    }
}
