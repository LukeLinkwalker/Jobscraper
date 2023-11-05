using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.Model.Data
{
    public class Ad
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string Title { get; set; }

        [NotNull]
        public string Company { get; set; }

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
            Title = string.Empty;
            URL = string.Empty;
            Timestamp = string.Empty;
            Content = string.Empty;
            Keywords = new List<string>();
        }

        public DateTime GetTimestamp()
        {
            return DateTime.Parse(Timestamp);
        }
    }
}
