using System;
using System.Text.RegularExpressions;

namespace Clarity.Core.Utilities 
{ 
    public static class ValidationUtils 
    { 
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        private static readonly Regex PhoneRegex = new Regex(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", RegexOptions.Compiled);

        public static bool IsValidEmail(string email) 
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return EmailRegex.IsMatch(email);
        }

        public static bool IsValidPhone(string phone)
        {
             if (string.IsNullOrWhiteSpace(phone)) return false;
             return PhoneRegex.IsMatch(phone);
        }
    } 
}
