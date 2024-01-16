namespace PetSitApp.DTOs.SitterSearchDTO
{
    public class ServiceDto
    {
        public int Id { get; set; }

        public int PetType { get; set; }

        public int SitterId { get; set; }
        public virtual ICollection<ServiceTypeDto> ServiceTypes { get; set; } = new List<ServiceTypeDto>();
    }
}
