using System;
using System.ComponentModel.DataAnnotations;

namespace ChanceForHappiness.Models
{
    public class Adoption
    {
        public int Id { get; set; }

        public int AnimalId { get; set; }

        public virtual Animal Animal { get; set; }

        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "На розгляді";

        [Required]
        [MaxLength(100)]
        public string ApplicantName { get; set; }

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
        [MaxLength(100)]
        public string HousingType { get; set; }

        public bool HasOwnHome { get; set; }

        [MaxLength(500)]
        public string LivingSituation { get; set; }

        [Required]
        [MaxLength(1000)]
        public string PreviousExperience { get; set; }

        public bool HasOtherPets { get; set; }

        [MaxLength(500)]
        public string OtherPetsDescription { get; set; }

        [Required]
        public int HoursAwayFromHome { get; set; }

        [Required]
        [MaxLength(1000)]
        public string AdoptionReason { get; set; }

        [MaxLength(100)]
        public string VeterinarianContact { get; set; }

        [MaxLength(2000)]
        public string AdditionalNotes { get; set; }

        [Required]
        public bool AgreedToTerms { get; set; }
    }
}