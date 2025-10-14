using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositiories.Interfaces
{
    public interface ITrainerRepositories
    {
        IEnumerable<Trainer> GetAll();
        Trainer? GetById(int Id);

        int Add(Trainer trainer);
        int Update(Trainer trainer);
        int Delete(Trainer trainer);
    }
}
