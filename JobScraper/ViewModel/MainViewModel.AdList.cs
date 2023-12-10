using JobScraper.Model.Data;
using JobScraper.Utils;
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
        private PubSub.Callback openTarget;

        private void InitAdList()
        {
            openTarget = OpenTarget;
            PubSub.Get().Subscribe(Topics.OPEN_TARGET, openTarget);
        }

        public void ForceUpdateAdList()
        {
            UpdateAdList();
        }

        private void UpdateAdList()
        {
            AdListArgs args = new AdListArgs();
            args.ads = _filteredAds;

            PubSub.Get().Publish(Topics.AD_PROCESSED, args);
        }

        private void UpdateAdList(List<Ad> ads)
        {
            AdListArgs args = new AdListArgs();
            args.ads = ads;

            PubSub.Get().Publish(Topics.AD_PROCESSED, args);
        }

        private void OpenTarget(dynamic data)
        {
            OpenTargetArgs args = data as OpenTargetArgs;

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = args.url,
                UseShellExecute = true
            };
            Process.Start(startInfo);
        }
    }
}
