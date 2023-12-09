using PetSitApp.Models;

namespace PetSitApp.ViewModels
{
    public class DashboardViewModel
    {
        public Owner? Owner { get; set; }
        public Sitter? Sitter { get; set; }
        public IEnumerable<Pet>? Pets { get; set; }
        public IEnumerable<Reservation>? Reservation { get; set; }
    }
}
