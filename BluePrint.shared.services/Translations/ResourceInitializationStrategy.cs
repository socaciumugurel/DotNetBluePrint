using BluePrint.core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace BluePrint.shared.services.Translations
{
    public class ResourceInitializationStrategy : IStrategy<IApplicationBuilder>
    {

        private readonly IStringLocalizerFactory _stringLocalizerFactory;
        private readonly ILocalizationService _localizationService;
        private const string TANSLATION_FILE_NAME = "Translations";

        public ResourceInitializationStrategy(IApplicationBuilder app)
        {
            _stringLocalizerFactory =
                (IStringLocalizerFactory)app.ApplicationServices.GetService(typeof(IStringLocalizerFactory))!;
            _localizationService =
                (ILocalizationService)app.ApplicationServices.GetService(typeof(ILocalizationService))!;
        }

        public static ResourceInitializationStrategy Create(IApplicationBuilder app)
        {
            return new ResourceInitializationStrategy(app);
        }

        public void Execute(IApplicationBuilder app)
        {
            var supportedCultures = new List<CultureInfo>
            {
                new (Constants.Languages.ENGLISH),
                new (Constants.Languages.FRENCH)
            };

            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(Constants.Languages.ENGLISH, Constants.Languages.ENGLISH),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            app.UseRequestLocalization(localizationOptions);

            var localizers = SetLocalizers(app);

            ValidateDuplications(localizers);
        }

        private List<IStringLocalizer> SetLocalizers(IApplicationBuilder app)
        {
            var location = GetType().Assembly.GetName().Name;

            if (location == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var localizers = new List<IStringLocalizer>
            {
                _stringLocalizerFactory.Create(TANSLATION_FILE_NAME, location),
            };

            _localizationService.SetLocalizers(localizers);
            return localizers;
        }

        private void ValidateDuplications(List<IStringLocalizer> localizers)
        {
            var localizedStrings = new List<LocalizedString>();

            localizers.ForEach(l => localizedStrings.AddRange((l.GetAllStrings())));

            var duplicates = localizedStrings
                .GroupBy(x => x.Name)
                .Where(g => g.Count() > 1)
                .Select(resource => resource.Key)
                .ToList();

            if (duplicates.Any())
            {
                throw new Exception($"Duplicate Resource: {string.Join(", ", duplicates)}");
            }
        }
    }
}
