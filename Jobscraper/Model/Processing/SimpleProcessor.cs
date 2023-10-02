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

        private List<Ad> _ads;
        private List<Keyword> _keywords;

        public SimpleProcessor(List<Ad> ads, List<Keyword> keywords)
        {
            _ads = ads;
            _keywords = keywords;
            ProcessAds();
        }

        public void SetKeywords(List<string> keywords)
        {

        }

        public void RegisterAdFetchedCallback(IScraper scraper)
        {
            scraper.OnAdFetchingProgress += (s, e) =>
            {
                AdFetchingProgressEvent pe = (AdFetchingProgressEvent)e;
                ProcessAd(pe.fetchedAd);
                _ads.Add(pe.fetchedAd);
            };
        }

        public void RegisterKeywordsChangedCallback()
        {
            // Use ProcessAds
        }

        private void ProcessAds()
        {
            foreach (Ad ad in _ads)
            {
                string txt = ad.Content.ToLower();
                ad.Keywords.Clear();

                foreach (Keyword keyword in _keywords)
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

            foreach (Keyword keyword in _keywords)
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
