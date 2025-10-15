using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositiories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositiories.Classes
{
    internal class MemberSessionRepository : IMemberSessionRepository
    {
        private readonly GymDbContext _dbContext;
        public MemberSessionRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Add(MemberSession memberSession)
        {
            _dbContext.MemberSessions.Add(memberSession);
            return _dbContext.SaveChanges();
        }

        public int Delete(MemberSession memberSession)
        {
            _dbContext.MemberSessions.Remove(memberSession);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<MemberSession> GetALl() => _dbContext.MemberSessions.ToList();


        public MemberSession? GetById(int Id) => _dbContext.MemberSessions.Find(Id);
       

        public int Update(MemberSession memberSession)
        {
            _dbContext.MemberSessions.Update(memberSession);
            return _dbContext.SaveChanges();
        }
    }
}
