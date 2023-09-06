﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

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
        [Range(1, 150,ErrorMessage ="Age between 1-150")]
        public int Age { get; set; }
        public byte[] ProfilePicture { get; set; }
        public ICollection<Pet>? Pets { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
