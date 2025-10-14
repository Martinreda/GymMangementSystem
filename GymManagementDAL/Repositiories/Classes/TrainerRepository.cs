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
    public class TrainerRepository : ITrainerRepositories
    {
        private readonly GymDbContext _dbContext;
        public TrainerRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Add(Trainer trainer)
        {
            _dbContext.Trainers.Add(trainer);
            return _dbContext.SaveChanges();
        }

        public int Delete(Trainer trainer)
        {
            _dbContext.Trainers.Remove(trainer);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Trainer> GetAll() => _dbContext.Trainers.ToList();



        public Trainer? GetById(int Id) => _dbContext.Trainers.Find(Id);
      

        public int Update(Trainer trainer)
        {
            _dbContext.Trainers.Update (trainer);
            return _dbContext.SaveChanges();
        }
    }
}
