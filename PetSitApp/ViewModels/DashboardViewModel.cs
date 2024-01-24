using PetSitApp.Models;

namespace PetSitApp.ViewModels
{
    public class DashboardViewModel
    {
        public Owner? Owner { get; set; }
        public IEnumerable<Sitter>? Sitters { get; set; }
        public IEnumerable<Pet>? Pets { get; set; }
        public IEnumerable<Reservation>? Reservations { get; set; }
    }
}
