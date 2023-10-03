using JobScraper.ViewModel.EventArgs;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.ViewModel
{
    public partial class MainViewModel
    {
        private void InitFilter()
        {
            OnKeywordAddReceived += HandleAddKeywordCallback;
            OnKeywordRemoveReceived += HandleRemoveKeywordCallback;
        }

        // Events for triggering callbacks from ViewModel -> GUI
        public event EventHandler<KeywordArg> OnKeywordAdded;
        public event EventHandler<KeywordArg> OnKeywordRemoved;

        // Events for receiving callbacks from GUI -> ViewModel
        private static event EventHandler<KeywordArg> OnKeywordAddReceived;
        private static event EventHandler<KeywordArg> OnKeywordRemoveReceived;

        /// <summary>
        /// EventCallback is used to receive callbacks from the GUI and hand control to a handler that is not subject to the same scoping limitations.
        /// Event chain is as follows:
        /// GUI Event -> AddKeywordCallback -> HandleAddKeywordCallback -> OnKeywordAdded
        /// </summary>
        public EventCallback<KeywordArg> AddKeywordCallback = new EventCallback<KeywordArg>(null, (KeywordArg args) =>
        {
            if(args.Keyword != null && args.Keyword.Length > 0)
            {
                OnKeywordAddReceived?.Invoke(null, args);
            }
        });

        /// <summary>
        /// Handler is used to interact with the underlying model and send the response from the model back to the GUI.
        /// Event chain is as follows:
        /// GUI Event -> AddKeywordCallback -> HandleAddKeywordCallback -> OnKeywordAdded
        /// </summary>
        private void HandleAddKeywordCallback(object? sender, KeywordArg args)
        {
            // Handle event

            // Send event to GUI
            OnKeywordAdded?.Invoke(this, new KeywordArg()
            {
                Keyword = args.Keyword
            });
        }

        /// <summary>
        /// EventCallback is used to receive callbacks from the GUI and hand control to a handler that is not subject to the same scoping limitations.
        /// Event chain is as follows:
        /// GUI Event -> RemoveKeywordCallback -> HandleRemoveKeywordCallback -> OnKeywordRemoved
        /// </summary>
        public EventCallback<KeywordArg> RemoveKeywordCallback = new EventCallback<KeywordArg>(null, (KeywordArg args) =>
        {
            OnKeywordRemoveReceived?.Invoke(null, args);
        });


        /// <summary>
        /// Handler is used to interact with the underlying model and send the response from the model back to the GUI.
        /// Event chain is as follows:
        /// GUI Event -> RemoveKeywordCallback -> HandleRemoveKeywordCallback -> OnKeywordRemoved
        /// </summary>
        private void HandleRemoveKeywordCallback(object? sender, KeywordArg args)
        {
            // Handle event

            // Send event to GUI
            OnKeywordRemoved?.Invoke(this, new KeywordArg()
            {
                Keyword = args.Keyword
            });
        }
    }
}
