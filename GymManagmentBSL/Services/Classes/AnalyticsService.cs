using GymManagementDAL.Entities;
using GymManagementDAL.Repositiories.Interfaces;
using GymManagmentBSL.Services.Interfaces;
using GymManagmentBSL.ViewModels.AnalyticsViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBSL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        
        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
    
        }

        public AnalyticsViewModel GetAnalyticsData()
        {
            var Sessions = _unitOfWork.SessionRepository.GetALl();

            return new AnalyticsViewModel
            {
                ActiveMembers = _unitOfWork.GetRepository<MemberShip>().GetALl( X => X.Status == "Active").Count(),
                TotalMembers = _unitOfWork.GetRepository<MemberShip>().GetALl().Count(),
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().GetALl().Count(),

                // Session Status Counts
                UpcomingSessions = Sessions.Count(predicate: X => X.StartDate > DateTime.Now),
                OngoingSessions = Sessions.Count(predicate: X => X.StartDate <= DateTime.Now && X.EndDate >= DateTime.Now),
                CompletedSessions = Sessions.Count(predicate: X => X.EndDate < DateTime.Now)
            };
        }
    }
}
