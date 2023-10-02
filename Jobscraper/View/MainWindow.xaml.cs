using Jobscraper.Model.Data;
using Jobscraper.Model.Processing;
using Jobscraper.Model.Scraping;
using Jobscraper.Model.Scraping.Events;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Jobscraper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var jobIndexScraper = new JobIndexScraper();

            jobIndexScraper.OnInitStarted += (s, e) => {
                MessageBox.Show("Started init!");
            };

            jobIndexScraper.OnInitDone += (s, e) => {
                MessageBox.Show("Init done!");
            };

            jobIndexScraper.OnAdScrapingStarted += (s, e) =>
            {
                MessageBox.Show("Scraping started!");
            };

            jobIndexScraper.OnAdScrapingDone += (s, e) => {
                MessageBox.Show("Scraping done!");
            };

            jobIndexScraper.OnAdFetchingStarted += (s, e) => {
                AdFetchingStartedEvent se = (AdFetchingStartedEvent)e;
                MessageBox.Show("Fetching ads started! : " + se.totalAds);
            };

            jobIndexScraper.OnAdFetchingProgress += (s, e) => { 
                AdFetchingProgressEvent pe = (AdFetchingProgressEvent)e;
                //MessageBox.Show(pe.index + " : " + pe.fetchedAd.Title);
            };

            var database = new Database();
            database.RegisterAdFetchedCallback(jobIndexScraper);

            List<Ad> ads = database.connection.GetAllWithChildren<Ad>();
            List<Keyword> keywords = database.connection.GetAllWithChildren<Keyword>();

            var processor = new SimpleProcessor(ads, keywords);
            processor.RegisterAdFetchedCallback(jobIndexScraper);

            jobIndexScraper.Init();
            jobIndexScraper.Start();


            //MessageBox.Show("Ads in database : " + ads.Count);
        }
    }
}
