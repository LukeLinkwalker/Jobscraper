using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Jobscraper.Model.Scraping;
using Jobscraper.Model.Scraping.Events;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace Jobscraper.Model.Data
{
    public class Database
    {
        private SQLiteConnection connection = null;

        public Database()
        {
            connection = new SQLiteConnection("data.db");
            connection.CreateTable<Ad>();
            connection.CreateTable<Keyword>();
        }

        public List<Keyword> GetKeywords()
        {
            return connection.GetAllWithChildren<Keyword>();
        }

        public List<Ad> GetAds()
        {
            return connection.GetAllWithChildren<Ad>();
        }

        public bool ContainsKeyword(string text)
        {
            return false;
        }

        public bool ContainsAd(string URL)
        {
            try
            {
                connection.Get<Ad>((ad) => ad.URL == URL);
            } 
            catch (System.InvalidOperationException ex)
            {
                return false;
            }

            return true;
        }

        public void SetAdFetchedCallback(IScraper scraper)
        {
            scraper.OnAdFetchingProgress += (s, e) =>
            {
                AdFetchingProgressEvent pe = (AdFetchingProgressEvent)e;
                connection.Insert(pe.fetchedAd);
                Debug.WriteLine("Added ad to db : " + pe.fetchedAd.Title);
            };
        }

        private void Clean()
        {
            // remove ads older than 90 days
        }
    }
}