using AutoMapper;
using GymManagementDAL.Entities;
using GymManagmentBSL.ViewModels.SessionViewModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBSL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Session, SessionViewModel>()
    // ... other members ...
    .ForMember(destinationMember: dest => dest.CategoryName, memberOptions: Options => Options.MapFrom(src => src.SessionCategory.CategoryName))
    .ForMember(destinationMember: dest => dest.TrainerName, memberOptions: Options => Options.MapFrom(src => src.SessionTrainer.Name))
            .ForMember(destinationMember: dest => dest.AvailableSlots, Options => Options.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();


        }
        

    }
}
