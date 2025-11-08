using AutoMapper;
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
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        //Ask Clr For Creaeting oblect from service 
        // builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); 
        // Clr Will inject Adderss of object in CTOr
        public MemberService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                if (IsEmailExists(createMember.Email) || IsPhoneExists(createMember.Phone))
                    return false;

                var member = _mapper.Map<Member>(createMember);

                _unitOfWork.GetRepository<Member>().Add(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetALl();
            if (members == null || !members.Any()) return Enumerable.Empty<MemberViewModel>();

            var memberViewModels = _mapper.Map<IEnumerable<MemberViewModel>>(members);
            return memberViewModels;
        }

        public MemberViewModel? GetMemberDetails(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member == null) return null;

            var viewModel = _mapper.Map<MemberViewModel>(member);

            var activeMembership = _unitOfWork.GetRepository<MemberShip>()
                .GetALl(x => x.MemberId == memberId && x.Status == "Active")
                .FirstOrDefault();

            if (activeMembership != null)
            {
                viewModel.MemberShipStartDate = activeMembership.CreatedAt.ToShortDateString();
                viewModel.MemberShipEndDate = activeMembership.EndDate.ToShortDateString();

                var plan = _unitOfWork.GetRepository<Plan>().GetById(activeMembership.PlanId);
                viewModel.PlanName = plan?.Name;
            }

            return viewModel;
        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
            var memberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (memberHealthRecord == null) return null;

            var healthRecordViewModel = _mapper.Map<HealthRecordViewModel>(memberHealthRecord);
            return healthRecordViewModel;
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null) return null;

            var updateViewModel = _mapper.Map<MemberToUpdateViewModel>(member);
            return updateViewModel;
        }

        public bool RemoveMember(int MemberId)
        {
            var memberRepo = _unitOfWork.GetRepository<Member>();
            var member = memberRepo.GetById(MemberId);
            if (member == null)
                return false;

            // تحقق هل لديه جلسات نشطة (تاريخ بداية الجلسة بعد الآن)
            var hasActiveMemberSessions = _unitOfWork.GetRepository<MemberSession>()
                .GetALl(x => x.MemberId == MemberId && x.session.StartDate > DateTime.Now)
                .Any();

            if (hasActiveMemberSessions)
                return false;

            var memberShipRepo = _unitOfWork.GetRepository<MemberShip>();
            var memberShips = memberShipRepo.GetALl(x => x.MemberId == MemberId);

            try
            {
                // حذف الاشتراكات المرتبطة أولاً
                if (memberShips.Any())
                {
                    foreach (var membership in memberShips)
                    {
                        memberShipRepo.Delete(membership);
                    }
                }

                // حذف العضو
                memberRepo.Delete(member);

                // حفظ التغييرات
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
                if (IsEmailExists(UpdatedMember.Email) || IsPhoneExists(UpdatedMember.Phone))
                    return false;

                var repo = _unitOfWork.GetRepository<Member>();
                var member = repo.GetById(Id);
                if (member == null)
                    return false;

                _mapper.Map(UpdatedMember, member);

                member.UpdatedAt = DateTime.Now;

                repo.Update(member);
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
