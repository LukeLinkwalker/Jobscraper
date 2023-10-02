using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobscraper.Data
{
    public class Ad
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Title { get; set; }

        [NotNull]
        public string URL { get; set; }

        [NotNull]
        public string Timestamp { get; set; }

        [NotNull]
        public string Content { get; set; }

        [Ignore]
        public List<string> Keywords { get; set; }

        public Ad()
        {
            this.Title = string.Empty;
            this.URL = string.Empty;
            this.Timestamp = string.Empty;
            this.Content = string.Empty;
            this.Keywords = new List<string>();
        }

        public DateTime getTimestamp()
        {
            return DateTime.Now;
        }
    }
}
