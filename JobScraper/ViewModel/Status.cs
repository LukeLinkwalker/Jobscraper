using JobScraper.Model.Scraping;
using JobScraper.Model.Scraping.Events;
using JobScraper.Utils;
using JobScraper.ViewModel.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.ViewModel
{
    public class Status
    {
        private int _numberOfAdsToScrape;
        private IScraper _scraper;

        public Status(IScraper scraper)
        {
            if (scraper != null)
            {
                _scraper = scraper;
                _scraper.OnInitStarted += _scraper_OnInitStarted;
                _scraper.OnInitDone += _scraper_OnInitDone;
                _scraper.OnAdScrapingStarted += _scraper_OnAdSrapingStarted;
                _scraper.OnAdScrapingDone += _scraper_OnAdScrapingDone;
                _scraper.OnAdFetchingStarted += _scraper_OnAdFectingStarted;
                _scraper.OnAdFetchingProgress += _scraper_OnAdFetchingProgress;
            }
        }

        private void _scraper_OnInitStarted(object? sender, System.EventArgs e)
        {
            EmitStatusChangeEvent(true, "Initializing");
        }

        private void _scraper_OnInitDone(object? sender, System.EventArgs e)
        {
            EmitStatusChangeEvent(false, "Initialized");
        }

        private void _scraper_OnAdSrapingStarted(object? sender, System.EventArgs e)
        {
            EmitStatusChangeEvent(true, "Finding ads");
        }

        private void _scraper_OnAdScrapingDone(object? sender, System.EventArgs e)
        {
            EmitStatusChangeEvent(false, "Scraping done");
        }

        private void _scraper_OnAdFectingStarted(object? sender, System.EventArgs e)
        {
            AdFetchingStartedEvent se = (AdFetchingStartedEvent)e;
            _numberOfAdsToScrape = se.totalAds;
            EmitStatusChangeEvent(true, "Scraping ad 1 of " + _numberOfAdsToScrape);
        }

        private void _scraper_OnAdFetchingProgress(object? sender, System.EventArgs e)
        {
            AdFetchingProgressEvent pe = (AdFetchingProgressEvent)e;
            EmitStatusChangeEvent(true, "Scraping ad " + (pe.index + 1) + " of " + _numberOfAdsToScrape);
        }

        /// <summary>
        /// Sends an event to the UI with information about updating the state of the status indicator
        /// </summary>
        /// <param name="visibility">Whether or not the status indicator should be visible</param>
        /// <param name="text">What text should be shown next to the status indicator</param>
        private void EmitStatusChangeEvent(bool visibility, string text)
        {
            StatusArgs args = new StatusArgs();
            args.visible = visibility;
            args.text = text;

            PubSub.Get().Publish(Topics.STATUS_CHANGED, args);
        }
    }
}
