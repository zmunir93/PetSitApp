namespace PetSitApp.DTOs.SitterSearchDTO
{
    public class DaysUnavailableDto
    {
        public int Id { get; set; }

        public int SitterId { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime Date { get; set; }
    }
}
