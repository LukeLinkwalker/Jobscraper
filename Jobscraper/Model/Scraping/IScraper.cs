using System;

namespace JobScraper.Model.Scraping
{
    public interface IScraper
    {
        event EventHandler OnInitStarted;
        event EventHandler OnInitDone;

        event EventHandler OnAdScrapingStarted;
        event EventHandler OnAdScrapingDone;

        event EventHandler OnAdFetchingStarted;
        event EventHandler OnAdFetchingDone;
        event EventHandler OnAdFetchingProgress;

        void Start();
        void Stop();
    }
}
