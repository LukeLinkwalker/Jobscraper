using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobscraper.Database
{
    public class Ad
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int ID { get; set; }

        [NotNull]
        public string Title { get; set; }

        [NotNull]
        public string URL { get; set; }

        [NotNull]
        public string Timestamp { get; set; }

        [OneToMany]
        public List<string> Keywords { get; set; }

        public Ad()
        {
            this.Title = string.Empty;
            this.URL = string.Empty;
            this.Timestamp = string.Empty;
            this.Keywords = new List<string>();
        }

        public DateTime getTimestamp()
        {
            return DateTime.Now;
        }
    }
}
