using PetSitApp.Models;

namespace PetSitApp.ViewModels
{
    public class AvailabilityViewModel
    {
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        public DateTime? SelectedDate { get; set; }
    }
}
