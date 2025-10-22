using GymManagementDAL.Entities;
using GymManagementDAL.Repositiories.Interfaces;
using GymManagmentBSL.Services.Interfaces;
using GymManagmentBSL.ViewModels.MemberViewModels;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBSL.Services.Classes
{
    internal class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<MemberShip> _membershipRepository;
        private readonly IPlanRepository _planRepository;

        //Ask Clr For Creaeting oblect from service 
        // builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); 
        // Clr Will inject Adderss of object in CTOr
        public MemberService(IGenericRepository<Member> memberRepository ,
            IGenericRepository<MemberShip> membershipRepository , 
             IPlanRepository planRepository )
        {
            _memberRepository = memberRepository;
            _membershipRepository = membershipRepository;
            _planRepository = planRepository;
        }

        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                //Check phone is exit or not
                var phoneExists = _memberRepository.GetALl(X => X.Phone == createMember.Phone).Any();

                //Check email is exit or not
                var emilExists = _memberRepository.GetALl(X => X.Email == createMember.Email).Any();

                //if one exists return false
                if (phoneExists || emilExists) return false;

                // If not add member and return true if added 
                var member = new Member()
                {
                    Name = createMember.Name,
                    Email = createMember.Email,
                    Phone = createMember.Phone,
                    Gender = createMember.Gender,
                    DateOfBirth = createMember.DateOfBirth,
                    Address = new Address()
                    {
                        BulidingNumber = createMember.BuildingNumber,
                        City = createMember.City,
                        Street = createMember.Street,
                    },

                    HealthRecord = new HealthRecord()
                    {
                        Weight = createMember.HealthRecordViewModel.Weight,
                        Height = createMember.HealthRecordViewModel.Height,
                        BloodType = createMember.HealthRecordViewModel.BloodType,
                        Note = createMember.HealthRecordViewModel.Note,

                    }
                };

                return _memberRepository.Add(member) > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Members = _memberRepository.GetALl();
            if (Members is null || !Members.Any()) return [];
            #region Manual Mapping 
            //var MemberViewModel = new List<MemberViewModel>();
            //foreach(var Member in Members)
            //{
            //    var memberViewModel = new MemberViewModel()
            //    {
            //        Id = Member.Id, 
            //        Name = Member.Name,
            //        Email = Member.Email,
            //        Phone = Member.Phone,
            //        Photo = Member.Photo,
            //        Gender = Member.Gender.ToString(),

            //    };
            //    MemberViewModel.Add(memberViewModel);
            //}
            #endregion
            #region Way02 
            var MemberViewModels = Members.Select(X => new MemberViewModel
            {
                Id = X.Id,
                Name = X.Name,
                Email = X.Email,
                Phone = X.Phone,
                Photo = X.Photo,
                Gender = X.Gender.ToString(),
            });

            #endregion
            return MemberViewModels;
        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var Member = _memberRepository.GetById(MemberId);
            if (Member is null) return null;

            var ViewModel = new MemberViewModel()
            {
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                Gender = Member.Gender.ToString(),
                DateOfBirth = Member.DateOfBirth.ToShortDateString(),
                Address = $"{Member.Address.BulidingNumber} _ {Member.Address.Street} _ {Member.Address.City}",
                Photo = Member.Photo,
            };

            //Active MemberShip 

            var ActiveMemberShip = _membershipRepository.GetALl(X => X.MemberId == MemberId && X.Status == "Active")
                                 .FirstOrDefault();

            if (ActiveMemberShip is not null)
            {
                ViewModel.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                ViewModel.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();

                var Plan = _planRepository.GetById(ActiveMemberShip.PlanId);
                ViewModel.PlanName = Plan?.Name;
            }
            return ViewModel;

        }
    }
}
