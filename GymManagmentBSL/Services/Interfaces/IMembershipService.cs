using GymManagmentBSL.ViewModels.MemberShipViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBSL.Services.Interfaces
{
    public interface IMembershipService
    {
        IEnumerable<MembershipViewModel> GetAllMemberships();
        IEnumerable<PlanSelectListViewModel> GetPlansForDropDown();
        IEnumerable<MemberSelectListViewModel> GetMembersForDropDown();
        bool CreateMembership(CreateMembershipViewModel model);
        bool DeleteMembership(int MemberId);
    }
}
