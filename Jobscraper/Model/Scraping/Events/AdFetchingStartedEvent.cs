using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobscraper.Model.Scraping.Events
{
    public class AdFetchingStartedEvent : EventArgs
    {
        public readonly int totalAds;

        public AdFetchingStartedEvent(int totalAds)
        {
            this.totalAds = totalAds;
        }
    }
}
