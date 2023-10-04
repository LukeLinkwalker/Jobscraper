﻿using JobScraper.Model.Scraping;
using JobScraper.Model.Scraping.Events;
using JobScraper.ViewModel.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.ViewModel
{
    public partial class MainViewModel
    {
        public event EventHandler<StatusArgs> OnStatusChange;

        private int _numberOfAdsToScrape;

        public void InitStatus()
        {
            _scraper.OnInitStarted += _scraper_OnInitStarted;
            _scraper.OnInitDone += _scraper_OnInitDone;
            _scraper.OnAdScrapingStarted += _scraper_OnAdSrapingStarted;
            _scraper.OnAdScrapingDone += _scraper_OnAdScrapingDone;
            _scraper.OnAdFetchingStarted += _scraper_OnAdFectingStarted;
            _scraper.OnAdFetchingProgress += _scraper_OnAdFetchingProgress;
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

        private void EmitStatusChangeEvent(bool visibility, string text)
        {
            StatusArgs args = new StatusArgs();
            args.Visible = visibility;
            args.Text = text;

            OnStatusChange?.Invoke(this, args);
        }

        

        

        
    }

    //_scraper.OnInitStarted += (s, e) =>
    //        {
    //            MessageBox.Show("Started init!");
    //        };
    //
    //jobIndexScraper.OnInitDone += (s, e) =>
    //{
    //    MessageBox.Show("Init done!");
    //};
    //
    //jobIndexScraper.OnAdScrapingStarted += (s, e) =>
    //{
    //    MessageBox.Show("Scraping started!");
    //};
    //
    //jobIndexScraper.OnAdScrapingDone += (s, e) =>
    //{
    //    MessageBox.Show("Scraping done!");
    //};
    //
    //jobIndexScraper.OnAdFetchingStarted += (s, e) =>
    //{
    //    AdFetchingStartedEvent se = (AdFetchingStartedEvent)e;
    //    MessageBox.Show("Fetching ads started! : " + se.totalAds);
    //};
    //
    //jobIndexScraper.OnAdFetchingProgress += (s, e) =>
    //{
    //    AdFetchingProgressEvent pe = (AdFetchingProgressEvent)e;
    //    MessageBox.Show("Fetched ad : " + pe.fetchedAd.Title);
    //};
}