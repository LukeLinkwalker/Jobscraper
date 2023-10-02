using Jobscraper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobscraper.Scraping.Events
{
    public class AdFetchingProgressEvent : EventArgs
    {
        public readonly int index;
        public readonly Ad fetchedAd;

        public AdFetchingProgressEvent(int index, Ad fetchedAd)
        {
            this.index = index;
            this.fetchedAd = fetchedAd;
        }
    }
}
