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
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly GymDbContext _dbContext;
        public HealthRecordRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Add(HealthRecord healthRecord)
        {
            _dbContext.HealthRecords.Add(healthRecord);
            return _dbContext.SaveChanges();
        }

        public int Delete(HealthRecord healthRecord)
        {
            _dbContext.HealthRecords.Remove(healthRecord);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<HealthRecord> GetALl() => _dbContext.HealthRecords.ToList();


        public HealthRecord? GetById(int Id) => _dbContext.HealthRecords.Find(Id);
       

        public int Update(HealthRecord healthRecord)
        {
            _dbContext.HealthRecords.Update(healthRecord);
            return _dbContext.SaveChanges();
        }
    }
}
