using AutoMapper;
using PetSitApp.DTOs.SitterSearchDTO;
using PetSitApp.Models;


namespace PetSitApp.AutoMapperProfiles
{
    
    

    public class SitterProfile : Profile
    {
        public SitterProfile()
        {
            // Define mappings here
            CreateMap<Sitter, SitterDto>();
            CreateMap<DaysUnavailable, DaysUnavailableDto>();
            CreateMap<WeekAvailability, WeekAvailabilityDto>();
        }
    }
}
