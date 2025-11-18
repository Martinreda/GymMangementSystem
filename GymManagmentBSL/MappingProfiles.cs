using AutoMapper;
using GymManagementDAL.Entities;
using GymManagmentBSL.ViewModels.MemberViewModels;
using GymManagmentBSL.ViewModels.PlanViewModels;
using GymManagmentBSL.ViewModels.SessionViewModel;
using GymManagmentBSL.ViewModels.TrianerViewModels;
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
            MapSession();
            MapMember();
            MapTrainer();
            MapPlan();

        }
        public void MapSession()
        {

            CreateMap<Session, SessionViewModel>()
    // ... other members ...
    .ForMember(destinationMember: dest => dest.CategoryName, memberOptions: Options => Options.MapFrom(src => src.SessionCategory.CategoryName))
    .ForMember(destinationMember: dest => dest.TrainerName, memberOptions: Options => Options.MapFrom(src => src.SessionTrainer.Name))
            .ForMember(destinationMember: dest => dest.AvailableSlots, Options => Options.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();


        }

        public void MapMember() 
        {
            CreateMap<CreateMemberViewModel, Member>()
    .ForMember(destinationMember: dest => dest.Address, memberOptions: opt => opt.MapFrom(mapExpression: src => src))
    .ForMember(destinationMember: dest => dest.HealthRecord, memberOptions: opt => opt.MapFrom(mapExpression: src => src.HealthRecord));

            CreateMap<CreateMemberViewModel, Address>()
                .ForMember(destinationMember: dest => dest.BulidingNumber, memberOptions: opt => opt.MapFrom(mapExpression: src => src.BuildingNumber))
                .ForMember(destinationMember: dest => dest.Street, memberOptions: opt => opt.MapFrom(mapExpression: src => src.Street))
                .ForMember(destinationMember: dest => dest.City, memberOptions: opt => opt.MapFrom(mapExpression: src => src.City));

            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();

            CreateMap<Member, MemberViewModel>()
                .ForMember(destinationMember: dest => dest.Gender, memberOptions: opt => opt.MapFrom(mapExpression: src => src.Gender.ToString()))
                .ForMember(destinationMember: dest => dest.DateOfBirth, memberOptions: opt => opt.MapFrom(mapExpression: src => src.DateOfBirth.ToShortDateString()))
                .ForMember(destinationMember: dest => dest.Address, memberOptions: opt => opt.MapFrom(mapExpression: src => $"{src.Address.BulidingNumber} {src.Address.Street}, {src.Address.City}"));

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(destinationMember: dest => dest.BuildingNumber, memberOptions: opt => opt.MapFrom(mapExpression: src => src.Address.BulidingNumber))
                .ForMember(destinationMember: dest => dest.Street, memberOptions: opt => opt.MapFrom(mapExpression: src => src.Address.Street))
                .ForMember(destinationMember: dest => dest.City, memberOptions: opt => opt.MapFrom(mapExpression: src => src.Address.City));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(destinationMember: dest => dest.Name, memberOptions: opt => opt.Ignore())
                .ForMember(destinationMember: dest => dest.Photo, memberOptions: opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BulidingNumber = src.BuildingNumber;
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                    dest.UpdatedAt = DateTime.Now;
                });
        }
        private void MapTrainer()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(destinationMember: dest => dest.Address,  opt => opt.MapFrom(mapExpression: src => new Address
                {
                    BulidingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }));

            CreateMap<Trainer, TrainerViewModel>();

            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(destinationMember: dest => dest.Street,  opt => opt.MapFrom(mapExpression: src => src.Address.Street))
                .ForMember(destinationMember: dest => dest.City,  opt => opt.MapFrom(mapExpression: src => src.Address.City))
                .ForMember(destinationMember: dest => dest.BuildingNumber,  opt => opt.MapFrom(mapExpression: src => src.Address.BulidingNumber));

            CreateMap<TrainerToUpdateViewModel, Trainer>()
                .ForMember(destinationMember: dest => dest.Name,  opt => opt.Ignore())
                .AfterMap(afterFunction: (src, dest) =>
                {
                    dest.Address.BulidingNumber = src.BuildingNumber;
                    dest.Address.City = src.City;
                    dest.Address.Street = src.Street;
                    dest.UpdatedAt = DateTime.Now;
                });
        }
        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();

            CreateMap<Plan, UpdatePlanViewModel>()
                .ForMember(destinationMember: dest => dest.PlanName, memberOptions: opt => opt.MapFrom(mapExpression: src => src.Name));

            CreateMap<UpdatePlanViewModel, Plan>()
                .ForMember(destinationMember: dest => dest.Name, memberOptions: opt => opt.Ignore())
                .ForMember(destinationMember: dest => dest.UpdatedAt, memberOptions: opt => opt.MapFrom(mapExpression: src => DateTime.Now));
        }



    }
}
