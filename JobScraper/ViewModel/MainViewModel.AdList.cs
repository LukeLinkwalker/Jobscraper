using JobScraper.Model.Data;
using JobScraper.ViewModel.EventArgs;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.ViewModel
{
    public partial class MainViewModel
    {
        // Events for triggering callbacks from ViewModel -> GUI
        public event EventHandler<AdListArgs> OnAdProcessed;

        // Events for receiving callbacks from GUI -> ViewModel
        private static event EventHandler<OpenTargetArgs> OnOpenTargetReceived;

        private void InitAdList()
        {
            OnOpenTargetReceived += HandleOpenTargetCallback;
        }

        public EventCallback<OpenTargetArgs> OpenTargetCallback = new EventCallback<OpenTargetArgs>(null, (OpenTargetArgs args) =>
        {
            OnOpenTargetReceived?.Invoke(null, args);
        });

        public void ForceUpdateAdList()
        {
            UpdateAdList();
        }

        private void UpdateAdList()
        {
            AdListArgs args = new AdListArgs();
            args.ads = filteredAds;

            OnAdProcessed.Invoke(null, args);
        }

        private void HandleOpenTargetCallback(object? sender, OpenTargetArgs args)
        {
            // Handle event
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = args.URL,
                UseShellExecute = true
            };
            Process.Start(startInfo);
        }
    }
}
