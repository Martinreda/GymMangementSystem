using GymManagementDAL.Entities;
using GymManagementDAL.Repositiories.Interfaces;
using GymManagmentBSL.Services.Interfaces;
using GymManagmentBSL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBSL.Services.Classes
{
    internal class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var Plans = _unitOfWork.GetRepository<Plan>().GetALl();
            if (Plans is null || !Plans.Any()) return [];
            return Plans.Select(P => new PlanViewModel()
            {
                Description = P.Description,
                DurationDays = P.DurationDays,
                Id = P.Id,
                IsActive = P.IsActive,
                Name = P.Name,
                Price = P.Price
            });
        }

        public PlanViewModel? GetPlanById(int planId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (Plan is null) return null;
            return new PlanViewModel()
            {
                Id = Plan.Id,
                Name = Plan.Name,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                IsActive = Plan.IsActive,
                Price = Plan.Price,
            };
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (Plan is null || Plan.IsActive == false || 
                HasActiveMemberShips(planId)) return null;

            return new UpdatePlanViewModel()
            {
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                PlanName = Plan.Name,
                Price = Plan.Price,
            };

        }
        public bool UpdatePlan(int planId, UpdatePlanViewModel updatedPlan)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (Plan is null || HasActiveMemberShips(planId)) return false;

            try
            {
                // description -- price -- duration days 
                (Plan.Description, Plan.Price, Plan.DurationDays, Plan.UpdatedAt) =
                    (updatedPlan.Description, updatedPlan.Price, updatedPlan.DurationDays, DateTime.Now);

                _unitOfWork.GetRepository<Plan>().Update(Plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
        public bool ToggleStatus(int planId)
        {
            var Repo = _unitOfWork.GetRepository<Plan>();
            var Plan = Repo.GetById(planId);
            if (Plan is null || HasActiveMemberShips(planId)) return false;
            Plan.IsActive = Plan.IsActive == true ? false : true;
            Plan.UpdatedAt = DateTime.Now;
            try
            {
                Repo.Update(Plan);
                return _unitOfWork.SaveChanges() > 0; 
            }
            catch
            {
                return false; 
            }
        }

       
        #region Helper 

        private bool HasActiveMemberShips(int planId)
        {
            var Activememberships = _unitOfWork.GetRepository<MemberShip>()
                .GetALl(X => X.PlanId == planId && X.Status == "Active");
            return Activememberships.Any();
        }
        #endregion
    }
}
