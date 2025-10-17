using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositiories.Interfaces
{
    internal interface IPlanRepository
    {
        Plan? GetById(int Id);
        IEnumerable<Plan> GetAll();
        int Update(Plan plan);
    }
}
