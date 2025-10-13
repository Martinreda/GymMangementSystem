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
    internal class MemberRepositiory : IMemberRepositiory
    {
        private readonly GymDbContext _dbContext = new GymDbContext();
        public int Add(Member member)
        {
            _dbContext.Members.Add(member);
            return _dbContext.SaveChanges();
        }

        public int Delete(int Id)
        {
            var Member = _dbContext.Members.Find(Id);
            if (Member is null) return 0;

            _dbContext.Members.Remove(Member);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Member> GetAll() => _dbContext.Members.ToList();


        public Member? GetById(int Id) => _dbContext.Members.Find(Id);


        public int Update(Member member)
        {
            _dbContext.Members.Update(member);
            return _dbContext.SaveChanges();
        }
    }
}
