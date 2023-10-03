using JobScraper.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.Model.Processing.Events
{
    public class AdsProcessedEvent : EventArgs
    {
        public readonly List<Ad> processedAds;

        public AdsProcessedEvent(List<Ad> processedAds)
        {
            this.processedAds = processedAds;
        }
    }
}
