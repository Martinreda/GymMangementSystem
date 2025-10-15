using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositiories.Interfaces
{
    public interface IMemberSessionRepository
    {
        IEnumerable<MemberSession> GetALl();
        MemberSession? GetById(int Id);

        int Add(MemberSession memberSession);
        int Update(MemberSession memberSession);
        int Delete(MemberSession memberSession);
    }
}
