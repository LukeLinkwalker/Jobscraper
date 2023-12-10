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
        public Database _database { get; set; }

        public MainViewModel(IScraper scraper, Database database)
        {
            _scraper = scraper;
            _database = database;
        }

        public void Init()
        {
            InitFilter();
            InitStatus();
            InitAdList();
        }
    }
}
