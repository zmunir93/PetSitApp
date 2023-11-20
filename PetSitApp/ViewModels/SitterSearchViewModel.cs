using PetSitApp.DTOs.SitterSearchDTO;

namespace PetSitApp.ViewModels
{
    public class SitterSearchViewModel
    {
        public IEnumerable<SitterDto> Sitters { get; set; }
        public string ApiKey { get; set; }
        public double ZipLat { get; set; }
        public double ZipLng { get; set; }

    }
}
