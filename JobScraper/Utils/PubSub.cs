using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.Utils
{
    public class PubSub
    {
        private static PubSub _instance = null;
        public delegate void Callback(dynamic data);
        private Dictionary<string, List<Callback>> subscriptions;

        public PubSub()
        {
            subscriptions = new Dictionary<string, List<Callback>>();
        }

        public static PubSub Get()
        {
            if (_instance == null)
            {
                _instance = new PubSub();
            }

            return _instance;
        }

        public void Subscribe(string topic, Callback callback)
        {
            if (subscriptions.ContainsKey(topic))
            {
                subscriptions[topic].Add(callback);
            }
            else
            {
                subscriptions.Add(topic, new List<Callback>());
                subscriptions[topic].Add(callback);
            }
        }

        public void Unsubscribe(string topic, Callback callback)
        {
            if (subscriptions.ContainsKey(topic))
            {
                subscriptions[topic].Remove(callback);

                if (subscriptions[topic].Count > 0)
                {
                    subscriptions.Remove(topic);
                }
            }
        }

        public void Publish(string topic, dynamic data)
        {
            if (subscriptions.ContainsKey(topic))
            {
                foreach (Callback callback in subscriptions[topic])
                {
                    callback(data);
                }
            }
        }
    }
}
