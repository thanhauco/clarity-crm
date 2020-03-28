using System;
using System.Text;

namespace Clarity.Core.Utilities 
{ 
    public static class StringExtensions 
    { 
        public static string Truncate(this string value, int maxLength) 
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }
        
        public static string ToSlug(this string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.ToLower().Replace(" ", "-").Replace(".", "");
        }
    } 
}
