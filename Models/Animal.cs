using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ChanceForHappiness.Models.Enums;

namespace ChanceForHappiness.Models
{
    public class Animal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public AnimalType Type { get; set; }

        [MaxLength(100)]
        public string Breed { get; set; }

        public int AgeYears { get; set; }

        public int AgeMonths { get; set; }

        public string Age
        {
            get
            {
                string yearsPart = AgeYears > 0 ? $"{AgeYears} {GetYearWord(AgeYears)}" : "";
                string monthsPart = AgeMonths > 0 ? $"{AgeMonths} {GetMonthWord(AgeMonths)}" : "";
                return string.Join(" ", new[] { yearsPart, monthsPart }.Where(p => !string.IsNullOrWhiteSpace(p)));
            }
        }

        private string GetYearWord(int number)
        {
            if (number % 10 == 1 && number % 100 != 11) return "рік";
            if (number % 10 >= 2 && number % 10 <= 4 && (number % 100 < 10 || number % 100 >= 20)) return "роки";
            return "років";
        }

        private string GetMonthWord(int number)
        {
            if (number % 10 == 1 && number % 100 != 11) return "місяць";
            if (number % 10 >= 2 && number % 10 <= 4 && (number % 100 < 10 || number % 100 >= 20)) return "місяці";
            return "місяців";
        }

        [MaxLength(1000)]
        public string Description { get; set; }

        public string Gender { get; set; }

        public double Weight { get; set; }

        public bool IsNeutered { get; set; }

        public AnimalStatus Status { get; set; }

        public DateTime ArrivalDate { get; set; }

        public string PhotoUrl { get; set; }

        [MaxLength(2000)]
        public string MedicalInfo { get; set; }

        [MaxLength(2000)]
        public string BehavioralNotes { get; set; }

        public bool GoodWithChildren { get; set; }

        public bool GoodWithOtherAnimals { get; set; }

        public bool IsHouseTrained { get; set; }

        public bool IsVaccinated { get; set; }

        public string Slug => $"{Id}-{Name.ToLower().Replace(" ", "-")}";
    }
}