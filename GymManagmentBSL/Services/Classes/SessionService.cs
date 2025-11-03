using GymManagementDAL.Entities;
using GymManagementDAL.Repositiories.Interfaces;
using GymManagmentBSL.Services.Interfaces;
using GymManagmentBSL.ViewModels.SessionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBSL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrianerAndCategory();
            if (!Sessions.Any()) return [];

            return Sessions.Select(S => new SessionViewModel
            {
                Id = S.Id,
                Description = S.Description,
                StartDate = S.StartDate,
                EndDate = S.EndDate,
                Capacity = S.Capacity,
                TrainerName = S.SessionTrainer.Name,
                CategoryName = S.SessionCategory.CategoryName,
            });
        }
    }
}
