using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobscraper.Model.Processing
{
    public interface IProcessor
    {
        event EventHandler OnAdProcessed;
        event EventHandler OnAdsProcessed;
    }
}
