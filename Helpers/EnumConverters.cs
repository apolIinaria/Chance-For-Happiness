using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ChanceForHappiness.Models.Enums;

namespace ChanceForHappiness.Helpers
{
    public class AnimalTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AnimalType animalType)
            {
                return animalType switch
                {
                    AnimalType.Dog => "Собака",
                    AnimalType.Cat => "Кіт",
                    AnimalType.Other => "Інше",
                    _ => "Невідомо",
                };
            }

            return "Невідомо";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue)
            {
                return strValue.ToLower() switch
                {
                    "Собака" => AnimalType.Dog,
                    "Кіт" => AnimalType.Cat,
                    _ => (object)AnimalType.Other,
                };
            }

            return DependencyProperty.UnsetValue;
        }
    }

    public class AnimalStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AnimalStatus status)
            {
                return status switch
                {
                    AnimalStatus.Available => "Доступно для прихисту",
                    AnimalStatus.Reserved => "Очікує на прихисток",
                    AnimalStatus.Adopted => "Прихищено",
                    AnimalStatus.InTreatment => "На лікуванні",
                    AnimalStatus.Quarantine => "На карантині",
                    _ => "Невідомо",
                };
            }

            return "Невідомо";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue)
            {
                return strValue.ToLower() switch
                {
                    "Доступно для прихисту" => AnimalStatus.Available,
                    "Очікує на прихисток" => AnimalStatus.Reserved,
                    "Прихищено" => AnimalStatus.Adopted,
                    "На лікуванні" => AnimalStatus.InTreatment,
                    "На карантині" => AnimalStatus.Quarantine,
                    _ => (object)AnimalStatus.Available,
                };
            }

            return DependencyProperty.UnsetValue;
        }
    }

    public class BoolToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? "Так" : "Ні";
            }

            return "Невідомо";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue)
            {
                return strValue.ToLower() == "Так";
            }

            return false;
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }

            return System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Windows.Visibility visibility)
            {
                return visibility == System.Windows.Visibility.Visible;
            }

            return false;
        }
    }

    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            return string.IsNullOrWhiteSpace(str) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                return count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}