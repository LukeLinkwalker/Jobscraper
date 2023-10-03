using JobScraper.ViewModel.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.ViewModel
{
    public class MainViewModel
    {
        public event EventHandler<KeywordArg> OnKeywordAdded;
        public event EventHandler<KeywordArg> OnKeywordRemoved;

        public void TriggerOnKeywordAdded(string keyword)
        {
            OnKeywordAdded?.Invoke(this, new KeywordArg()
            {
                Keyword = keyword
            });
        }

        public void TriggerOnKeywordRemoved(string keyword)
        {
            OnKeywordRemoved?.Invoke(this, new KeywordArg()
            {
                Keyword = keyword
            });
        }
    }
}
