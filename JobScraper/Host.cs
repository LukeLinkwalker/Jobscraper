using JobScraper.ViewModel.EventArgs;
using JobScraper.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using JobScraper.View.Pages;
using JobScraper.View.Components;
using JobScraper.Model.Processing;
using JobScraper.Model.Scraping;
using JobScraper.Model.Scraping.Events;
using JobScraper.Model.Data;

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

            var processor = new SimpleProcessor(database);
            processor.SetAdFetchedCallback(jobIndexScraper);

            MainViewModel viewModel = new MainViewModel();
            viewModel._scraper = jobIndexScraper;
            viewModel._database = database;
            viewModel._processor = processor;
            viewModel.Init();


            var services = new ServiceCollection();
            services.AddWindowsFormsBlazorWebView();
            blazorWebView.HostPage = "wwwroot\\index.html";
            blazorWebView.Services = services.BuildServiceProvider();
            blazorWebView.RootComponents.Add<Main>("#app",
                new Dictionary<string, object?>
                {
                    { "ViewModel", viewModel },
                    { "DoneLoadingCallback", new EventCallback(null, () => {
                        jobIndexScraper.Init();
                        jobIndexScraper.Start();
                    })}
                });


            //jobIndexScraper.Init();
            //jobIndexScraper.Start();
        }
    }
}