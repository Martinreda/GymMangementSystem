using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositiories.Interfaces
{
    public interface ISessionRepository
    {
        IEnumerable<Session> GetAll();

        Session? GetById(int Id);

        int Add(Session session);

        int Update(Session session);
        int Delete(Session session);

    }
}
