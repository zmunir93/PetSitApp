using PetSitApp.Models;

namespace PetSitApp.ViewModels
{
    public class SitterSearchViewModel
    {
        public IEnumerable<Sitter> Sitters { get; set; }
        public string ApiKey { get; set; }
    }
}
