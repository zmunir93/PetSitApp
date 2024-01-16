namespace PetSitApp.DTOs.SitterSearchDTO
{
    public class ServiceTypeDto
    {
        public int Id { get; set; }
        public string ServiceOffered { get; set; } = null!;
        public decimal Rate { get; set; }
    }
}
