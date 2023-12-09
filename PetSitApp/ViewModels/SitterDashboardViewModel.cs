using PetSitApp.Models;

namespace PetSitApp.ViewModels
{
    public class SitterDashboardViewModel
    {
        public Sitter? Sitter { get; set; }
        public Owner? Owner { get; set; }
        public IEnumerable<Reservation>? Reservations { get; set; }

    }
}
