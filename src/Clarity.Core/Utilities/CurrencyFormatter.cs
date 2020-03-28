using System;
using System.Globalization;

namespace Clarity.Core.Utilities 
{ 
    public static class CurrencyFormatter 
    { 
        public static string Format(decimal amount, string currencyCode = "USD")
        {
            var culture = CultureInfo.CurrentCulture;
            if (currencyCode == "USD") culture = new CultureInfo("en-US");
            else if (currencyCode == "EUR") culture = new CultureInfo("fr-FR");
            else if (currencyCode == "GBP") culture = new CultureInfo("en-GB");
            
            return amount.ToString("C", culture);
        }
    } 
}
