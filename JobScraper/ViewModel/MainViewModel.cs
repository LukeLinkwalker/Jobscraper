using JobScraper.Model.Data;
using JobScraper.Model.Scraping;
using JobScraper.ViewModel.EventArgs;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.ViewModel
{
    public partial class MainViewModel
    {
        public IScraper _scraper { get; set; }

        public MainViewModel(IScraper scraper, List<Ad> ads)
        {
            _scraper = scraper;
            _allAds = ads;
        }

        public void Init()
        {
            InitFilter();
            InitStatus();
            InitAdList();
        }
    }
}
