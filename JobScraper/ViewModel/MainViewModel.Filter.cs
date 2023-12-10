﻿using JobScraper.Model.Data;
using JobScraper.Model.Filter;
using JobScraper.Model.Scraping.Events;
using JobScraper.Utils;
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
        private PubSub.Callback addKeyword;
        private PubSub.Callback removeKeyword;

        private List<Ad> _allAds = new List<Ad>();
        private List<Ad> _filteredAds = new List<Ad>();
        private List<Keyword> _keywords = new List<Keyword>();

        private void InitFilter()
        {
            addKeyword = AddKeyword;
            removeKeyword = RemoveKeyword;

            PubSub.Get().Subscribe(Topics.ADD_KEYWORD, addKeyword);
            PubSub.Get().Subscribe(Topics.REMOVE_KEYWORD, removeKeyword);

            _scraper.OnAdFetchingProgress += HandleOnAdFetchingProgressEvent;

            _allAds = _database.GetAds();
            _filteredAds = _allAds.OrderByDescending(ad => ad.GetTimestamp()).ToList();
        }

        private void AddKeyword(dynamic data) 
        {
            FilterArgs args = data as FilterArgs;

            // Remove keyword if it is already in the list of keywords
            Keyword? keyword = _keywords.Where(keyword => keyword.text.ToLower() == args.keyword.text.ToLower()).SingleOrDefault();
            if(keyword != null)
            {
                _keywords.Remove(keyword);
                PubSub.Get().Publish(Topics.REMOVED_KEYWORD, new FilterArgs { keyword = args.keyword });
            }

            _keywords.Add(args.keyword);

            FilterAds();

            PubSub.Get().Publish(Topics.ADDED_KEYWORD, new FilterArgs { keyword = args.keyword });
        }

        private void RemoveKeyword(dynamic data)
        {
            FilterArgs args = data as FilterArgs;

            Keyword keyword = _keywords.Single(k => k.text == args.keyword.text);
            _keywords.Remove(keyword);
            FilterAds();

            PubSub.Get().Publish(Topics.REMOVED_KEYWORD, new FilterArgs { keyword = args.keyword });
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

                    foreach (Keyword keyword in _keywords)
                    {
                        if (txt.Contains(keyword.text.ToLower()))
                        {
                            ad.Keywords.Add(keyword);
                        }
                    }
                }

                _filteredAds = _allAds.ToList();

                if(_keywords.Where(keyword => keyword.type == Keyword.Type.Must).Count() > 0) {
                    // Set _filteredAds to only ads that have atleast one keyword
                    _filteredAds = _filteredAds.Where(ad => ad.Keywords.Where(keyword => keyword.type == Keyword.Type.Must).Count() > 0).ToList();
                }

                if(_keywords.Where(keyword => keyword.type == Keyword.Type.Cannot).Count() > 0) {
                    // Set _filteredAds to only ads that don't have keywords of type Cannot
                    _filteredAds = _filteredAds.Where(ad => ad.Keywords.Where(keyword => keyword.type == Keyword.Type.Cannot).Count() == 0).ToList();
                }
            }
            // Sort ads by timestamp
            _filteredAds = _filteredAds.OrderByDescending(ad => ad.GetTimestamp()).ToList();

            UpdateAdList();
        }
    }
}
