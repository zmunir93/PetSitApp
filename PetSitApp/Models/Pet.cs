using System.ComponentModel.DataAnnotations;

namespace PetSitApp.Models
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Species { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }

        [Required]
        public string Breed { get; set; }

        public bool Chipped { get; set; }

        public bool Spayed { get; set; }

        public bool Friendly { get; set; }

        public DateTime AdoptionDate { get; set; }
        [MaxLength(500)]
        public string About { get; set; }
        [MaxLength(500)]
        public string Care { get; set; }

        public bool PottyTrained { get; set; }

        public string EnergyLevel { get; set; }
        [MaxLength(500)]
        public string FeedingSchedule { get; set; }

        public string LeftAlone { get; set; }
        [MaxLength(500)]
        public string MedicalInformation { get; set; }
        [MaxLength(500)]
        public string AdditionalInfo { get; set; }
        [MaxLength(500)]
        public string VetInformation { get; set; }

        // Foreign Key
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
    }

}
