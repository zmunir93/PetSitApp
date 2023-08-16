using System.ComponentModel.DataAnnotations;

namespace PetSitApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required] 
        public string Password { get; set; }

        public ICollection<Permission>? Permissions { get; set; }
        public Owner Owner { get; set; }
    }
}
