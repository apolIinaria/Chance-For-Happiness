using System.Globalization;
using System.Windows.Controls;

namespace ChanceForHappiness.Helpers
{
    /// <summary>
    /// Клас для валідації номерів телефону у застосунку.
    /// Перевіряє, чи номер телефону є коректним українським номером.
    /// Наслідується від ValidationRule для інтеграції з системою валідації WPF.
    /// </summary>
    public class PhoneValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string phone = value?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(phone))
                return new ValidationResult(false, "Номер телефону є обов'язковим.");

            if (!ValidationHelpers.IsValidPhoneNumber(phone))
                return new ValidationResult(false, "Будь ласка, введіть дійсний український номер телефону (+380XXXXXXXXX або 0XXXXXXXXX).");

            return ValidationResult.ValidResult;
        }
    }
}