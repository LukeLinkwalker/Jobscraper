﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobscraper.Processing
{
    public class SimpleProcessor : IProcessor
    {
        public event EventHandler OnAdProcessed;
    }
}
