using System.ComponentModel.DataAnnotations;

namespace PetSitApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required] 
        public string LastName { get; set; }

        public ICollection<Permission>? Permissions { get; set; }
    }
}
