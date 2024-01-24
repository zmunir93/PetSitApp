using PetSitApp.Models;

namespace PetSitApp.ViewModels
{
    public class SuccessViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Sitter Sitter { get; set; }

    }
}
