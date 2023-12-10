using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScraper.Utils
{
    public static class Topics
    {
        // Filter topics
        public const string ADD_KEYWORD = "ADD_KEYWORD";
        public const string ADDED_KEYWORD = "ADDED_KEYWORD";
        public const string REMOVE_KEYWORD = "REMOVE_KEYWORD";
        public const string REMOVED_KEYWORD = "REMOVED_KEYWORD";

        // Ad list topics
        public const string OPEN_TARGET = "OPEN_TARGET";
        public const string AD_PROCESSED = "AD_PROCESSED";

        // Status topics
        public const string STATUS_CHANGED = "STATUS_CHANGED";
    }
}
