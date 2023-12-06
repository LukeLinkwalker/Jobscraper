using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.Model.Filter
{
    public class Keyword
    {
        public string text { get; set; }
        public Type type { get; set; }

        public enum Type
        {
            Must = 0,
            Should = 1,
            Cannot = 2
        }
    }
}
