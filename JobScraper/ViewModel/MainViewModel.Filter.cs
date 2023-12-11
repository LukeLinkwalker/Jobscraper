using JobScraper.Model.Data;
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
        private List<Ad> _allAds = new List<Ad>();
        private List<Ad> _filteredAds = new List<Ad>();
        private List<Keyword> _keywords = new List<Keyword>();

        private void InitFilter()
        {
            PubSub.Get().Subscribe(Topics.ADD_KEYWORD, AddKeyword);
            PubSub.Get().Subscribe(Topics.REMOVE_KEYWORD, RemoveKeyword);

            if(_scraper != null)
            {
                _scraper.OnAdFetchingProgress += HandleOnAdFetchingProgressEvent;
            }

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

            List<Ad> ads = FilterAds(_allAds, _keywords);
            UpdateAdList(ads);

            PubSub.Get().Publish(Topics.ADDED_KEYWORD, new FilterArgs { keyword = args.keyword });
        }

        private void RemoveKeyword(dynamic data)
        {
            FilterArgs args = data as FilterArgs;

            Keyword keyword = _keywords.Single(k => k.text == args.keyword.text);
            _keywords.Remove(keyword);

            List<Ad> ads = FilterAds(_allAds, _keywords);
            UpdateAdList(ads);

            PubSub.Get().Publish(Topics.REMOVED_KEYWORD, new FilterArgs { keyword = args.keyword });
        }

        private void HandleOnAdFetchingProgressEvent(object? sender, System.EventArgs e)
        {
            AdFetchingProgressEvent pe = (AdFetchingProgressEvent) e;
            _allAds.Add(pe.fetchedAd);

            List<Ad> ads = FilterAds(_allAds, _keywords);
            UpdateAdList(ads);
        }

        public List<Ad> FilterAds(List<Ad> ads, List<Keyword> keywords)
        {
            List<Ad> filteredAds = ads;

            if (keywords.Count > 0)
            {
                // Find keywords in ads
                foreach (Ad ad in filteredAds)
                {
                    string txt = ad.Content.ToLower();
                    ad.Keywords.Clear();

                    foreach (Keyword keyword in keywords)
                    {
                        if (txt.Contains(keyword.text.ToLower()))
                        {
                            ad.Keywords.Add(keyword);
                        }
                    }
                }

                if (keywords.Where(keyword => keyword.type == Keyword.Type.Must).Count() > 0)
                {
                    // Set filteredAds to only ads that have atleast one keyword
                    filteredAds = filteredAds.Where(ad => ad.Keywords.Where(keyword => keyword.type == Keyword.Type.Must).Count() > 0).ToList();
                }

                if (keywords.Where(keyword => keyword.type == Keyword.Type.Cannot).Count() > 0)
                {
                    // Set filteredAds to only ads that don't have keywords of type Cannot
                    filteredAds = filteredAds.Where(ad => ad.Keywords.Where(keyword => keyword.type == Keyword.Type.Cannot).Count() == 0).ToList();
                }
            }
            // Sort ads by timestamp
            filteredAds = filteredAds.OrderByDescending(ad => ad.GetTimestamp()).ToList();

            return filteredAds;
        }
    }
}
