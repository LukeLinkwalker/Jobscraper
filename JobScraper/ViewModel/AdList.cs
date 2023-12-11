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
    public class AdList
    {
        public AdList()
        {
            PubSub.Get().Subscribe(Topics.OPEN_TARGET, OpenTarget);
        }

        public void OpenTarget(dynamic data)
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
