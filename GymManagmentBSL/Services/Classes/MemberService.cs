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
        private readonly IUnitOfWork _unitOfWork;


        //Ask Clr For Creaeting oblect from service 
        // builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); 
        // Clr Will inject Adderss of object in CTOr
        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                ////Check phone is exit or not
                //var phoneExists = _memberRepository.GetALl(X => X.Phone == createMember.Phone).Any();

                ////Check email is exit or not
                //var emilExists = _memberRepository.GetALl(X => X.Email == createMember.Email).Any();

                //if one exists return false
                if (IsEmailExists (createMember.Email) || IsPhoneExists (createMember.Phone)) 
                    return false;

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

                 _unitOfWork.GetRepository<Member>().Add(member);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Members = _unitOfWork.GetRepository<Member>().GetALl();
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
            var Member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
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

            var ActiveMemberShip = _unitOfWork.GetRepository<MemberShip>().GetALl(X => X.MemberId == MemberId && X.Status == "Active")
                                 .FirstOrDefault();

            if (ActiveMemberShip is not null)
            {
                ViewModel.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                ViewModel.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();

                var Plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId);
                ViewModel.PlanName = Plan?.Name;
            }
            return ViewModel;

        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
            var MemberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (MemberHealthRecord is null) return null;

            return new HealthRecordViewModel()
            {
                BloodType = MemberHealthRecord.BloodType,
                Height = MemberHealthRecord.Height,
                Note = MemberHealthRecord.Note,
                Weight = MemberHealthRecord.Weight,
            };
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return null;
            return new MemberToUpdateViewModel()
            {
                  Email = Member.Email,
                  Name = Member.Name,
                  Phone = Member.Phone,
                  Photo = Member.Photo,
                  BuildingNumber = Member.Address.BulidingNumber,
                  City = Member.Address.City,
                  Street = Member.Address.Street,
            };
        }

        public bool RemoveMember(int MemberId)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();
            var Member = MemberRepo.GetById(MemberId);
            if (Member is null) return false;

            var HasActiveMemberSessions = _unitOfWork.GetRepository<MemberSession>()
                .GetALl(X => X.MemberId == MemberId && X.session.StartDate > DateTime.Now).Any();
            if (HasActiveMemberSessions) return false;
            var MemberShipRepo = _unitOfWork.GetRepository<MemberShip>();
            var MemberShips = MemberShipRepo.GetALl(X => X.MemberId == MemberId);
            try
            {
                if (MemberShips.Any ())
                {
                    foreach (var membership in MemberShips)
                    {
                        MemberShipRepo.Delete(membership);
                    }
                }
                MemberRepo.Delete(Member) ;
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel UpdatedMember)
        {
            try
            {
                //var EmailExists = _memberRepository.GetALl(X => X.Email == UpdatedMember.Email).Any();
                //var PhoneExists = _memberRepository.GetALl(X => X.Phone == UpdatedMember.Phone).Any();
                // No Duplcation -======= Clean code 

                if (IsEmailExists (UpdatedMember.Email) || IsPhoneExists (UpdatedMember.Phone))
                    return false;
                var repo = _unitOfWork.GetRepository<Member>();
                var member = repo.GetById(Id);
                if (member == null)
                    return false;

                member.Email = UpdatedMember.Email;
                member.Phone = UpdatedMember.Phone;

                member.Address.BulidingNumber = UpdatedMember.BuildingNumber;
                member.Address.City = UpdatedMember.City;
                member.Address.Street = UpdatedMember.Street;

                member.UpdatedAt = DateTime.Now;

                 repo.Update(member) ;
                return _unitOfWork.SaveChanges() > 0; 


            }
            catch
            {
                return false;
            }
        }

        #region Helper Methods 

        private bool IsEmailExists (string email)
        {
            return _unitOfWork.GetRepository<Member>().GetALl(X => X.Email == email).Any(); 
        }
        private bool IsPhoneExists(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetALl(X => X.Phone == phone).Any();
        }

        #endregion
    }
}
