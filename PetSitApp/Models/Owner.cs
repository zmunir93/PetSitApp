using System.ComponentModel.DataAnnotations;

namespace PetSitApp.Models
{
    public class Owner
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public int Age { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Pet> Pets { get; set; }
    }
}
