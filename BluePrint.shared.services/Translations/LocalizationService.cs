using BluePrint.core.Infrastructure;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace BluePrint.shared.services.Translations
{
    [Service(typeof(ILocalizationService), Lifetime.Singleton)]
    public class LocalizationService : ILocalizationService
    {
        private List<IStringLocalizer> _localizers = new();

        public void SetLocalizers(List<IStringLocalizer> localizers)
        {
            _localizers = localizers;
        }

        public string this[string key] => Translate(key);

        public string Translate(string key)
        {
            var localization = _localizers.FirstOrDefault(x => !x[key].ResourceNotFound);

            if (localization != null)
            {
                return localization[key];
            }

            return key;
        }

        public string Translate(string key, string culture)
        {
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
            return this[key];
        }

        public LocalizedHtmlString TranslateToHtml(string key)
        {
            var localization = _localizers.FirstOrDefault(x => !x[key].ResourceNotFound);

            if (localization != null)
            {
                return new LocalizedHtmlString(key, localization[key]);
            }

            return new LocalizedHtmlString(key, key, true);
        }
    }
}
