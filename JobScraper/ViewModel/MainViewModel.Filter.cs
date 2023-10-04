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
        // Events for triggering callbacks from ViewModel -> GUI
        public event EventHandler<FilterArgs> OnKeywordAdded;
        public event EventHandler<FilterArgs> OnKeywordRemoved;

        // Events for receiving callbacks from GUI -> ViewModel
        private static event EventHandler<FilterArgs> OnKeywordAddReceived;
        private static event EventHandler<FilterArgs> OnKeywordRemoveReceived;

        private void InitFilter()
        {
            OnKeywordAddReceived += HandleAddKeywordCallback;
            OnKeywordRemoveReceived += HandleRemoveKeywordCallback;
        }

        /// <summary>
        /// EventCallback is used to receive callbacks from the GUI and hand control to a handler that is not subject to the same scoping limitations.
        /// Event chain is as follows:
        /// GUI Event -> AddKeywordCallback -> HandleAddKeywordCallback -> OnKeywordAdded
        /// </summary>
        public EventCallback<FilterArgs> AddKeywordCallback = new EventCallback<FilterArgs>(null, (FilterArgs args) =>
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
        private void HandleAddKeywordCallback(object? sender, FilterArgs args)
        {
            // Handle event
            _processor.AddKeyword(args.Keyword);

            // Send event to GUI
            OnKeywordAdded?.Invoke(this, new FilterArgs()
            {
                Keyword = args.Keyword
            });
        }

        /// <summary>
        /// EventCallback is used to receive callbacks from the GUI and hand control to a handler that is not subject to the same scoping limitations.
        /// Event chain is as follows:
        /// GUI Event -> RemoveKeywordCallback -> HandleRemoveKeywordCallback -> OnKeywordRemoved
        /// </summary>
        public EventCallback<FilterArgs> RemoveKeywordCallback = new EventCallback<FilterArgs>(null, (FilterArgs args) =>
        {
            OnKeywordRemoveReceived?.Invoke(null, args);
        });


        /// <summary>
        /// Handler is used to interact with the underlying model and send the response from the model back to the GUI.
        /// Event chain is as follows:
        /// GUI Event -> RemoveKeywordCallback -> HandleRemoveKeywordCallback -> OnKeywordRemoved
        /// </summary>
        private void HandleRemoveKeywordCallback(object? sender, FilterArgs args)
        {
            // Handle event
            _processor.RemoveKeyword(args.Keyword);

            // Send event to GUI
            OnKeywordRemoved?.Invoke(this, new FilterArgs()
            {
                Keyword = args.Keyword
            });
        }
    }
}
