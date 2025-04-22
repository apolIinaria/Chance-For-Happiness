using System;
using System.ComponentModel.DataAnnotations;

namespace ChanceForHappiness.Models
{
    public class Volunteer
    {
        public int Id { get; set; }

        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "На розгляді";

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(500)]
        public string Address { get; set; }

        [Required]
        [Range(16, 100)]
        public int Age { get; set; }

        [Required]
        [MaxLength(500)]
        public string Availability { get; set; }

        [Required]
        [MaxLength(500)]
        public string PreferredActivities { get; set; }

        [MaxLength(1000)]
        public string PreviousExperience { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Motivation { get; set; }

        [MaxLength(1000)]
        public string Skills { get; set; }

        [MaxLength(2000)]
        public string AdditionalInformation { get; set; }

        [Required]
        [MaxLength(100)]
        public string EmergencyContact { get; set; }

        [Required]
        public bool AgreedToTerms { get; set; }
    }
}