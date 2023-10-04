using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.Model.Processing
{
    public interface IProcessor
    {
        event EventHandler OnAdsProcessed;

        void AddKeyword(string keyword);
        void RemoveKeyword(string keyword);
        void ForceProcess();
    }
}
