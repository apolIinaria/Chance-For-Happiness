using System.Globalization;
using System.Windows.Controls;

namespace ChanceForHappiness.Helpers
{
    /// <summary>
    /// Клас для валідації електронної пошти у WPF застосунку.
    /// Наслідується від ValidationRule, що дозволяє використовувати його 
    /// для перевірки даних, які вводить користувач у формі.
    /// </summary>
    public class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string email = value?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(email))
                return new ValidationResult(false, "Електронна пошта обов'язкова.");

            if (!ValidationHelpers.IsValidEmail(email))
                return new ValidationResult(false, "Будь ласка, введіть дійсну електронну адресу.");

            return ValidationResult.ValidResult;
        }
    }
}