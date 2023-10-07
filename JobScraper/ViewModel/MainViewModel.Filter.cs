using JobScraper.Model.Data;
using JobScraper.Model.Scraping.Events;
using JobScraper.ViewModel.EventArgs;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
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

        private List<Ad> allAds = new List<Ad>();
        private List<Ad> filteredAds = new List<Ad>();
        private List<string> keywords = new List<string>();

        private void InitFilter()
        {
            OnKeywordAddReceived += HandleAddKeywordCallback;
            OnKeywordRemoveReceived += HandleRemoveKeywordCallback;

            allAds = _database.GetAds();
            filteredAds = allAds.ToList();
        }

        private void _scraper_OnAdFetchingProgress_Filter(object? sender, System.EventArgs e)
        {
            AdFetchingProgressEvent pe = (AdFetchingProgressEvent) e;
            allAds.Add(pe.fetchedAd);
            FilterAds();
        }

        private void FilterAds()
        {
            filteredAds.Clear();
            filteredAds = allAds.FindAll(ad => ad.Keywords.Count == 0).ToList();

            if (keywords.Count > 0)
            {
                // Find keywords in ads
                foreach (Ad ad in filteredAds)
                {
                    string txt = ad.Content.ToLower();
                    ad.Keywords.Clear();

                    foreach (string keyword in keywords)
                    {
                        if (txt.Contains(keyword.ToLower()))
                        {
                            ad.Keywords.Add(keyword);
                        }
                    }
                }

                // Remove ads without any matching keywords
                for (int i = filteredAds.Count - 1; i >= 0; i -= 1)
                {
                    if (filteredAds[i].Keywords.Count == 0)
                    {
                        filteredAds.RemoveAt(i);
                    }
                }
            }

            // Sort ads by timestamp
            filteredAds = filteredAds.OrderByDescending(ad => ad.Timestamp).ToList();

            UpdateAdList();
        }

        /// <summary>
        /// EventCallback is used to receive callbacks from the GUI and hand control to a handler that is not subject to the same scoping limitations.
        /// Event chain is as follows:
        /// GUI Event -> AddKeywordCallback -> HandleAddKeywordCallback -> OnKeywordAdded
        /// </summary>
        public EventCallback<FilterArgs> AddKeywordCallback = new EventCallback<FilterArgs>(null, (FilterArgs args) =>
        {
            if(args.Keyword != null && args.Keyword.Length > 0)
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
            keywords.Add(args.Keyword);
            FilterAds();

            // Send event to GUI
            OnKeywordAdded?.Invoke(this, new FilterArgs()
            {
                Keyword = args.Keyword
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
            keywords.Remove(args.Keyword);
            FilterAds();

            // Send event to GUI
            OnKeywordRemoved?.Invoke(this, new FilterArgs()
            {
                Keyword = args.Keyword
            });
        }
    }
}
