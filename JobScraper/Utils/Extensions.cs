using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static SQLite.SQLite3;

namespace JobScraper.Utils
{
    public static class Extensions
    {
        /// <summary>
        /// Finds the element that match the element parameter and returns the property value for that element.
        /// </summary>
        /// <param name="handle">Element to be searched</param>
        /// <param name="element">Element to be found</param>
        /// <param name="property">Property to be returned</param>
        /// <returns></returns>
        public static async Task<string> QueryElementAndProperty(this IElementHandle handle, string element, string property)
        {
            IElementHandle queriedElement = await handle.QuerySelectorAsync(element);
            IJSHandle queriedProperty = await queriedElement.GetPropertyAsync(property);
            return queriedProperty.RemoteObject.Value.ToString();
        }

        /// <summary>
        /// Finds the element that match the element parameter and returns the property value for that element.
        /// </summary>
        /// <param name="handle">Element to be searched</param>
        /// <param name="element">Element to be found</param>
        /// <param name="property">Property to be returned</param>
        /// <returns></returns>
        public static async Task<string> QueryElementAndProperty(this IPage page, string element, string property)
        {
            IElementHandle queriedElement = await page.QuerySelectorAsync(element);
            IJSHandle queriedProperty = await queriedElement.GetPropertyAsync(property);
            return queriedProperty.RemoteObject.Value.ToString();
        }

        /// <summary>
        /// Finds all elements that match the element parameter and returns the property value for all elements.
        /// </summary>
        /// <param name="handle">Element to be searched</param>
        /// <param name="element">Element to be found</param>
        /// <param name="property">Property to be returned</param>
        /// <returns></returns>
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
