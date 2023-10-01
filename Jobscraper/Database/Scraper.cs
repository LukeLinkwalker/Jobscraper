using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobscraper.Database
{
    public class Scraper
    {
        [PrimaryKey, NotNull]
        public string ID { get; set; }

        [NotNull]
        public string Timestamp { get; set; }

        public Scraper()
        {
            this.Timestamp = string.Empty;
        }

        public DateTime getTimestamp()
        {
            return DateTime.Now;
        }
    }
}
