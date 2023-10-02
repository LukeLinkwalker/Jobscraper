﻿using Jobscraper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobscraper.Scraping.Events
{
    public class AdProcessedEvent
    {
        public Ad processedAd;

        public AdProcessedEvent(Ad processedAd)
        {
            this.processedAd = processedAd;
        }
    }
}
