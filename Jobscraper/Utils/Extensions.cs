using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static SQLite.SQLite3;

namespace Jobscraper
{
    public static class Extensions
    {
        public static async Task<string> QueryElementAndProperty(this IElementHandle handle, string element, string property)
        {
            IElementHandle queriedElement = await handle.QuerySelectorAsync(element);
            IJSHandle queriedProperty = await queriedElement.GetPropertyAsync(property);
            return queriedProperty.RemoteObject.Value.ToString();
        }

        public static async Task<string> QueryElementAndProperty(this IPage page, string element, string property)
        {
            IElementHandle queriedElement = await page.QuerySelectorAsync(element);
            IJSHandle queriedProperty = await queriedElement.GetPropertyAsync(property);
            return queriedProperty.RemoteObject.Value.ToString();
        }

        public static async Task<List<string>> QueryAllElementsAndProperties(this IPage page, string element, string property)
        {
            List<string> properties = new List<string>();

            IElementHandle[] queriedElements = await page.QuerySelectorAllAsync(element);

            foreach(IElementHandle elementHandle in queriedElements)
            {
                IJSHandle queriedProperty = await elementHandle.GetPropertyAsync(property);
                properties.Add(queriedProperty.RemoteObject.Value.ToString());
            }

            return properties;
        }
    }
}
