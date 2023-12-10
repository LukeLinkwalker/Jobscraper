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

            MainViewModel viewModel = new MainViewModel(jobIndexScraper, database);
            viewModel.Init();

            var services = new ServiceCollection();
            services.AddWindowsFormsBlazorWebView();

            services.AddBlazorWebViewDeveloperTools();

            blazorWebView.HostPage = "wwwroot\\index.html";
            blazorWebView.Services = services.BuildServiceProvider();
            blazorWebView.RootComponents.Add<Main>("#app",
                new Dictionary<string, object?>
                {
                    { "viewModel", viewModel },
                    { "DoneLoadingCallback", new EventCallback(null, () => {
                        viewModel.ForceUpdateAdList();
                        jobIndexScraper.Init();
                        jobIndexScraper.Start();
                    })}
                });
        }
    }
}