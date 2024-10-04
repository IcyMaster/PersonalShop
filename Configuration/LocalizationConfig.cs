using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace PersonalShop.Configuration;

public static class LocalizationConfig
{
    public static void RegisterLocalization(this WebApplication app)
    {
        var supportedCultures = new[]
        {
            new CultureInfo("fa-IR")
            {
                NumberFormat = {
                    CurrencySymbol = "تومان ", // currency display symbol
                    CurrencyDecimalSeparator=".",
                    NumberDecimalSeparator=".",
                    CurrencyDecimalDigits = 0, // decimal places digits (ex: 1000/00)
                    NumberDecimalDigits = 0,
                }
            },
            new CultureInfo("en-US")
        };

        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("fa-IR"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures,
            ApplyCurrentCultureToResponseHeaders = true
        };

        // Use the localization settings in the application
        app.UseRequestLocalization(localizationOptions);
    }
}
