using Jobscraper.Model.Data;
using Jobscraper.Model.Processing.Events;
using Jobscraper.Model.Scraping;
using Jobscraper.Model.Scraping.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobscraper.Model.Processing
{
    public class SimpleProcessor : IProcessor
    {
        public event EventHandler OnAdProcessed;
        public event EventHandler OnAdsProcessed;

        private Database _database;
        private List<Ad> _ads;

        public SimpleProcessor(Database database)
        {
            _database = database; 
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

                foreach (Keyword keyword in _database.GetKeywords())
                {
                    if (txt.Contains(keyword.Text.ToLower()))
                    {
                        ad.Keywords.Add(keyword.Text);
                    }
                }
            }

            OnAdsProcessed?.Invoke(this, new AdsProcessedEvent(_ads));
        }

        private void ProcessAd(Ad ad)
        {
            string txt = ad.Content.ToLower();
            ad.Keywords.Clear();

            foreach (Keyword keyword in _database.GetKeywords())
            {
                if (txt.Contains(keyword.Text.ToLower()))
                {
                    ad.Keywords.Add(keyword.Text);
                }
            }

            OnAdProcessed?.Invoke(this, new AdProcessedEvent(ad));
        }
    }
}
