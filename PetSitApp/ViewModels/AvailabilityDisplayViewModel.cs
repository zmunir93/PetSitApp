﻿namespace PetSitApp.ViewModels
{
    public class AvailabilityDisplayViewModel
    {
            public bool Monday { get; set; }
            public bool Tuesday { get; set; }
            public bool Wednesday { get; set; }
            public bool Thursday { get; set; }
            public bool Friday { get; set; }
            public bool Saturday { get; set; }
            public bool Sunday { get; set; }

            public List<DateTime>? SelectedDates { get; set; }
        
    }
}
