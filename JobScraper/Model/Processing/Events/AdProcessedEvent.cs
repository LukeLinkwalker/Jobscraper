using JobScraper.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.Model.Processing.Events
{
    public class AdProcessedEvent : EventArgs
    {
        public readonly Ad processedAd;

        public AdProcessedEvent(Ad processedAd)
        {
            this.processedAd = processedAd;
        }
    }
}
