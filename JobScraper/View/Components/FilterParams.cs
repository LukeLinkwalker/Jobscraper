using JobScraper.ViewModel.EventArgs;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.View.Components
{
    public class FilterParams
    {
        public EventCallback<KeywordArg> AddKeywordCallback;
        public EventCallback<KeywordArg> RemoveKeywordCallback;
    }
}
