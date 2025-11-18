using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositiories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositiories.Classes
{
    public class MembershipRepository : GenericRepository<MemberShip>, IMembershipRepository
    {
        private readonly GymDbContext _context;

        public MembershipRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<MemberShip> GetAllMembershipsWithMembersAndPlans(Expression<Func<MemberShip, bool>>? filter = null)
        {
            IQueryable<MemberShip> query = _context.Memberships
                .Include(m => m.member)
                .Include(m => m.Plan);

            if (filter != null)
                query = query.Where(filter);

            return query.ToList();
        }


        public IEnumerable<MemberShip> GetAllMembershipsWithMembersAndPlans(Func<MemberShip, bool>? filter = null)
        {
            var memberships = _context.Memberships.Include(m => m.member).Include(m => m.Plan)
                            .Where(filter ?? (_ => true));

            return memberships;

        }

        public MemberShip? GetFirstOrDefault(Func<MemberShip, bool>? filter = null)
        {
            var membership = _context.Memberships.Include(m => m.member).Include(m => m.Plan)
                            .FirstOrDefault(filter ?? (_ => true));
            return membership;
        }

    }
}
