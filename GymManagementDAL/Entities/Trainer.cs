using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Trainer : GymUser
    {
        // HireDate == Created AT of BaseEntity use FluentApi
        public Specialties Specialties { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Session> TrainerSessions { get; set; } = null!;
        
    }
}
