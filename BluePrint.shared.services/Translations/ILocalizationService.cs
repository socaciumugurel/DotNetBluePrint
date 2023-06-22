using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace BluePrint.shared.services.Translations
{
    public interface ILocalizationService
    {
        /// <summary>
        /// Sets the StringLocalizers
        /// </summary>
        /// <param name="localizers"></param>
        public void SetLocalizers(List<IStringLocalizer> localizers);

        /// <summary>
        /// Gets the resource value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string this[string key] { get; }

        /// <summary>
        /// Gets the resource value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Translate(string key);

        /// <summary>
        /// Gets the resource value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        string Translate(string key, string culture);

        public LocalizedHtmlString TranslateToHtml(string key);
    }
}
