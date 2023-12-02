using PetSitApp.Models;

namespace PetSitApp.ViewModels
{
    public class SitterCheckoutDashViewModel
    {
        public Sitter Sitter { get; set; }
        public int Id { get; set; }
        public string PetType { get; set; }
        public string ServiceType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
