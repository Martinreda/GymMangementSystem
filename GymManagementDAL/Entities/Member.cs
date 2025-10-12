using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    internal class Member : GymUser
    {
        //JoinDate == CratedAt of BaseEntity
        public string? Photo { get; set; }

        #region RelationShips 
        #region Member _ HealthRecord 
        public HealthRecord HealthRecord { get; set; } = null!;
        #endregion

        #region Member - MemberShip 
        public ICollection<MemberShip> memberShips { get; set; }
        #endregion
        #region Member - MemberSession 
        public ICollection<MemberSession> memberSessions { get; set; }
        #endregion
        #endregion
    }
}
