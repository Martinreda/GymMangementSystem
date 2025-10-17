using GymManagementDAL.Entities;
using GymManagementDAL.Repositiories.Interfaces;
using GymManagmentBSL.Services.Interfaces;
using GymManagmentBSL.ViewModels.MemberViewModels;
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

        //Ask Clr For Creaeting oblect from service 
        // builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); 
        // Clr Will inject Adderss of object in CTOr
        public MemberService(IGenericRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
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
    }
}
