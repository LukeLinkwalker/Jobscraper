﻿using JobScraper.Model.Data;
using JobScraper.Model.Scraping.Events;
using JobScraper.ViewModel.EventArgs;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.ViewModel
{
    public partial class MainViewModel
    {
        // Events for triggering callbacks from ViewModel -> GUI
        public event EventHandler<FilterArgs> OnKeywordAdded;
        public event EventHandler<FilterArgs> OnKeywordRemoved;

        // Events for receiving callbacks from GUI -> ViewModel
        private static event EventHandler<FilterArgs> OnKeywordAddReceived;
        private static event EventHandler<FilterArgs> OnKeywordRemoveReceived;

        private List<Ad> _allAds = new List<Ad>();
        private List<Ad> _filteredAds = new List<Ad>();
        private List<string> _keywords = new List<string>();

        private void InitFilter()
        {
            OnKeywordAddReceived += HandleAddKeywordCallback;
            OnKeywordRemoveReceived += HandleRemoveKeywordCallback;

            _scraper.OnAdFetchingProgress += HandleOnAdFetchingProgressEvent;

            _allAds = _database.GetAds();
            _filteredAds = _allAds.OrderByDescending(ad => ad.GetTimestamp()).ToList();
        }

        private void HandleOnAdFetchingProgressEvent(object? sender, System.EventArgs e)
        {
            AdFetchingProgressEvent pe = (AdFetchingProgressEvent) e;
            _allAds.Add(pe.fetchedAd);
            FilterAds();
        }

        /// <summary>
        /// Filters all ads stored based on the keywords stored
        /// </summary>
        private void FilterAds()
        {
            _filteredAds = _allAds;

            foreach(Ad ad in _allAds)
            {
                ad.Keywords.Clear();
            }

            if (_keywords.Count > 0)
            {
                // Find keywords in ads
                foreach (Ad ad in _filteredAds)
                {
                    string txt = ad.Content.ToLower();
                    ad.Keywords.Clear();

                    foreach (string keyword in _keywords)
                    {
                        if (txt.Contains(keyword.ToLower()))
                        {
                            ad.Keywords.Add(keyword);
                        }
                    }
                }

                // Set filtered ads to only be ads that have keywords
                _filteredAds = _allAds.Where(ad => ad.Keywords.Count > 0).ToList();
            }

            // Sort ads by timestamp
            _filteredAds = _filteredAds.OrderByDescending(ad => ad.GetTimestamp()).ToList();

            UpdateAdList();
        }

        /// <summary>
        /// EventCallback is used to receive callbacks from the GUI and hand control to a handler that is not subject to the same scoping limitations.
        /// Event chain is as follows:
        /// GUI Event -> AddKeywordCallback -> HandleAddKeywordCallback -> OnKeywordAdded
        /// </summary>
        public EventCallback<FilterArgs> AddKeywordCallback = new EventCallback<FilterArgs>(null, (FilterArgs args) =>
        {
            if(args.keyword != null && args.keyword.Length > 0)
            {
                OnKeywordAddReceived?.Invoke(null, args);
            }
        });

        /// <summary>
        /// Handler is used to interact with the underlying model and send the response from the model back to the GUI.
        /// Event chain is as follows:
        /// GUI Event -> AddKeywordCallback -> HandleAddKeywordCallback -> OnKeywordAdded
        /// </summary>
        private void HandleAddKeywordCallback(object? sender, FilterArgs args)
        {
            // Handle event
            _keywords.Add(args.keyword);
            FilterAds();

            // Send event to GUI
            OnKeywordAdded?.Invoke(this, new FilterArgs()
            {
                keyword = args.keyword
            });
        }

        /// <summary>
        /// EventCallback is used to receive callbacks from the GUI and hand control to a handler that is not subject to the same scoping limitations.
        /// Event chain is as follows:
        /// GUI Event -> RemoveKeywordCallback -> HandleRemoveKeywordCallback -> OnKeywordRemoved
        /// </summary>
        public EventCallback<FilterArgs> RemoveKeywordCallback = new EventCallback<FilterArgs>(null, (FilterArgs args) =>
        {
            OnKeywordRemoveReceived?.Invoke(null, args);
        });


        /// <summary>
        /// Handler is used to interact with the underlying model and send the response from the model back to the GUI.
        /// Event chain is as follows:
        /// GUI Event -> RemoveKeywordCallback -> HandleRemoveKeywordCallback -> OnKeywordRemoved
        /// </summary>
        private void HandleRemoveKeywordCallback(object? sender, FilterArgs args)
        {
            // Handle event
            _keywords.Remove(args.keyword);
            FilterAds();

            // Send event to GUI
            OnKeywordRemoved?.Invoke(this, new FilterArgs()
            {
                keyword = args.keyword
            });
        }
    }
}
