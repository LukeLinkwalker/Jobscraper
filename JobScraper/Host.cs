using JobScraper.ViewModel.EventArgs;
using JobScraper.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using JobScraper.View.Pages;
using JobScraper.View.Components;
using JobScraper.Model.Scraping;
using JobScraper.Model.Scraping.Events;
using JobScraper.Model.Data;

using AdList = JobScraper.ViewModel.AdList;
using Filter = JobScraper.ViewModel.Filter;
using Status = JobScraper.ViewModel.Status;

namespace JobScraper
{
    public partial class Host : Form
    {
        public Host()
        {
            InitializeComponent();

            var database = new Database();
            var jobIndexScraper = new JobIndexScraper(database);

            database.SetAdFetchedCallback(jobIndexScraper);

            AdList adList = new AdList();
            Filter filter = new Filter(jobIndexScraper, database.GetAds());
            Status status = new Status(jobIndexScraper);

            var services = new ServiceCollection();
            services.AddWindowsFormsBlazorWebView();
            services.AddBlazorWebViewDeveloperTools();

            blazorWebView.HostPage = "wwwroot\\index.html";
            blazorWebView.Services = services.BuildServiceProvider();
            blazorWebView.RootComponents.Add<Main>("#app",
                new Dictionary<string, object?>
                {
                    { "DoneLoadingCallback", new EventCallback(null, () => {
                        filter.ForceUpdateAdList();
                        jobIndexScraper.Init();
                        jobIndexScraper.Start();
                    })}
                });
        }
    }
}