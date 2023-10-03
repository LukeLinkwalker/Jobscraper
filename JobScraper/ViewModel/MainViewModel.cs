using JobScraper.Model.Data;
using JobScraper.ViewModel.EventArgs;
using Microsoft.AspNetCore.Components;
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

        // Events for handing off from lambda expression to handler
        private static event EventHandler<KeywordArg> OnKeywordAddReceived;
        private static event EventHandler<KeywordArg> OnKeywordRemoveReceived;

        public MainViewModel()
        {
            OnKeywordAddReceived += HandleAddKeywordCallback;
            OnKeywordRemoveReceived += HandleRemoveKeywordCallback;
        }

        // Used to handle event and send event back to UI
        public void HandleRemoveKeywordCallback(KeywordArg args)
        {
            TriggerOnKeywordAdded(args.Keyword);
        }

        // Where event from UI is first received
        public EventCallback<KeywordArg> AddKeywordCallback = new EventCallback<KeywordArg>(null, (KeywordArg args) =>
        {
            OnKeywordAddReceived?.Invoke(null, args);
        });

        private void HandleAddKeywordCallback(object? sender, KeywordArg args)
        {
            TriggerOnKeywordAdded(args.Keyword);
        }

        public EventCallback<KeywordArg> RemoveKeywordCallback = new EventCallback<KeywordArg>(null, (KeywordArg args) =>
        {
            OnKeywordRemoveReceived?.Invoke(null, args);
        });

        private void HandleRemoveKeywordCallback(object? sender, KeywordArg args)
        {
            TriggerOnKeywordRemoved(args.Keyword);
        }

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
