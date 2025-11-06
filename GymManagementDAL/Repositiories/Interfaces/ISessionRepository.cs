using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositiories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSessionsWithTrianerAndCategory();
        int GetCountOfBooksSlots(int sessionId);

        Session? GeTSessionWithTrainerAndCategory(int SessionId);
    }
}
