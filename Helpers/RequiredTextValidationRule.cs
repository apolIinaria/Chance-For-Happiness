using System.Globalization;
using System.Windows.Controls;

namespace ChanceForHappiness.Helpers
{
    /// <summary>
    /// Клас для валідації обов'язкових текстових полів у WPF застосунку.
    /// Перевіряє наявність тексту та обмежує його максимальну довжину.
    /// Наслідується від ValidationRule для інтеграції з системою валідації WPF.
    /// </summary>
    public class RequiredTextValidationRule : ValidationRule
    {
        public int MaxLength { get; set; } = 1000;
        public string FieldName { get; set; } = "Field";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = value?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(text))
                return new ValidationResult(false, $"{FieldName} обов'язково");

            if (text.Length > MaxLength)
                return new ValidationResult(false, $"{FieldName} має бути меньше {MaxLength} символів.");

            return ValidationResult.ValidResult;
        }
    }
}