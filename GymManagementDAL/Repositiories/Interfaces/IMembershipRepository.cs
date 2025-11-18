using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositiories.Interfaces
{
    public interface IMembershipRepository : IGenericRepository<MemberShip>
    {
        IEnumerable<MemberShip> GetAllMembershipsWithMembersAndPlans(Func<MemberShip, bool>? filter = null);
        MemberShip? GetFirstOrDefault(Func<MemberShip, bool>? filter = null);
    }
}
