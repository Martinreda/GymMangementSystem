using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositiories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositiories.Classes
{
    public class BookingRepository : GenericRepository<MemberSession>, IBookingRepository
    {
        private readonly GymDbContext _context;

        public BookingRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<MemberSession> GetSessionById(int sessionId)
        {
            return _context.MemberSessions.Where(ms => ms.SessionId == sessionId)
                                          .Include(ms => ms.Member)
                                          .ToList();

        }
    }
}
