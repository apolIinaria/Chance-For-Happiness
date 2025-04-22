using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ChanceForHappiness.Helpers
{
    public static class ValidationHelpers
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            string pattern = @"^(\+380\d{9}|0\d{9})$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        public static bool IsValidText(string text, int maxLength = 1000)
        {
            return !string.IsNullOrWhiteSpace(text) && text.Length <= maxLength;
        }

        public static bool IsValidAge(int age, int minAge = 16, int maxAge = 100)
        {
            return age >= minAge && age <= maxAge;
        }
    }
}