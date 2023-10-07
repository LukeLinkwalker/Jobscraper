using JobScraper.Model.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.Model.Scraping
{
    internal class BaseScraper : IScraper
    {
        public event EventHandler OnInitStarted;
        public event EventHandler OnInitDone;
        public event EventHandler OnAdScrapingStarted;
        public event EventHandler OnAdScrapingDone;
        public event EventHandler OnAdFetchingStarted;
        public event EventHandler OnAdFetchingDone;
        public event EventHandler OnAdFetchingProgress;

        public bool IsRunning { get; private set; }

        private Database _database;

        public BaseScraper(Database database) {
            _database = database;
        }

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        private async Task<int> FindPages()
        {
            return 0;
        }

        private async Task<List<Ad>> FindAds()
        {
            return new List<Ad>();
        }

        private async Task<string> FetchAdContent(string URL)
        {
            return string.Empty;
        }
    }
}
