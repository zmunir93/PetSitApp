using System.ComponentModel.DataAnnotations;

namespace PetSitApp.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Role { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }


    }
}