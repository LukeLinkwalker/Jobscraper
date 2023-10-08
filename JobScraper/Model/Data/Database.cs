using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using JobScraper.Model.Scraping;
using JobScraper.Model.Scraping.Events;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace JobScraper.Model.Data
{
    public class Database
    {
        private SQLiteConnection _connection;

        public Database()
        {
            _connection = new SQLiteConnection("data.db");
            _connection.CreateTable<Ad>();

            // Remove ads in database older than 60 days
            DateTime currentTime = DateTime.Now;
            var ads = _connection.GetAllWithChildren<Ad>(); 
            foreach(Ad ad in ads.Where(ad => currentTime.Subtract(ad.GetTimestamp()).Days > 60))
            {
                _connection.Delete<Ad>(ad);
            }
        }

        /// <summary>
        /// Returns all ads in the database
        /// </summary>
        public List<Ad> GetAds()
        {
            return _connection.GetAllWithChildren<Ad>();
        }

        /// <summary>
        /// Checks if the database already contains an ad
        /// </summary>
        public bool ContainsAd(string URL)
        {
            try
            {
                _connection.Get<Ad>(ad => ad.URL == URL);
            } 
            catch (System.InvalidOperationException ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Binds the database to the AdFetchedCallback invoked by the scraper when a new ad has been scraped.
        /// </summary>
        public void SetAdFetchedCallback(IScraper scraper)
        {
            scraper.OnAdFetchingProgress += (s, e) =>
            {
                AdFetchingProgressEvent pe = (AdFetchingProgressEvent)e;
                _connection.Insert(pe.fetchedAd);
            };
        }
    }
}