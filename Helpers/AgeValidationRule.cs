using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChanceForHappiness.Helpers
{
    public class AgeValidationRule : ValidationRule
    {
        public int MinAge { get; set; } = 16;
        public int MaxAge { get; set; } = 100;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || !int.TryParse(value.ToString(), out int age))
                return new ValidationResult(false, "Будь ласка, введіть дійсний вік");

            if (!ValidationHelpers.IsValidAge(age, MinAge, MaxAge))
                return new ValidationResult(false, $"Вік має бути від {MinAge} до {MaxAge}.");

            return ValidationResult.ValidResult;
        }
    }
}