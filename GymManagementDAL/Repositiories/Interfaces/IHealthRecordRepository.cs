using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositiories.Interfaces
{
    internal interface IHealthRecordRepository
    {
        IEnumerable<HealthRecord> GetALl();
        HealthRecord? GetById(int Id);

        int Add(HealthRecord healthRecord);
        int Update(HealthRecord healthRecord);
        int Delete (HealthRecord healthRecord);
    }
}
