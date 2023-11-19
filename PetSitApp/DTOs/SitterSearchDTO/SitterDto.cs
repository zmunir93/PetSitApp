using PetSitApp.Models;

namespace PetSitApp.DTOs.SitterSearchDTO
{
    public class SitterDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public byte[]? ProfilePicture { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int Age { get; set; }

        public string Address { get; set; } = null!;

        public string City { get; set; } = null!;

        public string State { get; set; } = null!;

        public string Zip { get; set; } = null!;

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public virtual ICollection<DaysUnavailableDto> DaysUnavailables { get; set; } = new List<DaysUnavailableDto>();

        public virtual Service? Service { get; set; }

        public virtual User User { get; set; } = null!;

        public virtual WeekAvailabilityDto? WeekAvailability { get; set; }
    }
}
