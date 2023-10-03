using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.Model.Data
{
    public class Keyword
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Text { get; set; }

        public Keyword(string text)
        {
            Text = text;
        }

        public Keyword() 
        {
            Text = string.Empty;
        }
    }
}
