using JobScraper.Model.Data;
using JobScraper.Model.Processing.Events;
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
            _processor.OnAdsProcessed += _processor_OnAdsProcessed;
        }

        public EventCallback<OpenTargetArgs> OpenTargetCallback = new EventCallback<OpenTargetArgs>(null, (OpenTargetArgs args) =>
        {
            OnOpenTargetReceived?.Invoke(null, args);
        });

        private void _processor_OnAdsProcessed(object? sender, System.EventArgs e)
        {
            AdsProcessedEvent pe = (AdsProcessedEvent)e;

            AdListArgs args = new AdListArgs();
            args.ads = pe.processedAds;

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
