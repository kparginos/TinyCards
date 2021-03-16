using System;
using System.Collections.Generic;

namespace TinyBank.Core.Constants
{
    public static class Country
    {
        public const string GreekCountryCode = "GR";
        public const string CyprusCountryCode = "CY";
        public const string ItalyCountryCode = "IT";

        public static readonly IReadOnlyCollection<string> SupportedCountryCodes =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                GreekCountryCode,
                CyprusCountryCode,
                ItalyCountryCode
            };
    }
}
