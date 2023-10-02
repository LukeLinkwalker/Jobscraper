using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Jobscraper.Scraping;
using Jobscraper.Scraping.Events;
using SQLite;

namespace Jobscraper.Data
{
    public class Database
    {
        public readonly SQLiteConnection connection = null;

        public Database()
        {
            connection = new SQLiteConnection("data.db");
            connection.CreateTable<Ad>();
            connection.CreateTable<Keyword>();
        }

        public bool ContainsAd(string URL)
        {
            return false;
        }

        public void RegisterAdFetchedEvent(IScraper scraper)
        {
            scraper.OnAdFetchingProgress += (s, e) => { 
                AdFetchingProgressEvent pe = (AdFetchingProgressEvent)e;
                connection.Insert(pe.fetchedAd);
            };
        }
    }
}
