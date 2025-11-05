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
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext; 
        public SessionRepository(GymDbContext dbContext) : base (dbContext)
        {
            _dbContext = dbContext;
        }

        

        public IEnumerable<Session> GetAllSessionsWithTrianerAndCategory()
        {
            return _dbContext.Sessions.Include(X=> X.SessionTrainer)
                .Include(navigationPropertyPath: X => X.SessionCategory)
                .ToList();
        }

        public int GetCountOfBooksSlots(int sessionId)
        {
            return _dbContext.MemberSessions.Count(X => X.SessionId == sessionId);
        }

        public Session? GeTSessionWithTrainerAndCategory(int SessionId)
        {
            return _dbContext.Sessions
                     .Include(navigationPropertyPath: X => X.SessionTrainer)
                     .Include(navigationPropertyPath: X => X.SessionCategory)
                     .FirstOrDefault(predicate: X => X.Id == SessionId);
        }
    }
}
