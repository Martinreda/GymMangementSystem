using AutoMapper;
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
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetALl();
            if (plans == null || !plans.Any()) return Enumerable.Empty<PlanViewModel>();

            // استخدم AutoMapper للتحويل
            return _mapper.Map<IEnumerable<PlanViewModel>>(plans);
        }

        public PlanViewModel? GetPlanById(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan == null) return null;

            return _mapper.Map<PlanViewModel>(plan);
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan == null || !plan.IsActive || HasActiveMemberShips(planId)) return null;

            return _mapper.Map<UpdatePlanViewModel>(plan);
        }

        public bool UpdatePlan(int planId, UpdatePlanViewModel updatedPlan)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan == null || HasActiveMemberShips(planId)) return false;

            try
            {
                // استخدم AutoMapper لتحديث الخصائص بدل التعيين اليدوي
                _mapper.Map(updatedPlan, plan);

                plan.UpdatedAt = DateTime.Now;

                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool ToggleStatus(int planId)
        {
            var repo = _unitOfWork.GetRepository<Plan>();
            var plan = repo.GetById(planId);
            if (plan == null || HasActiveMemberShips(planId)) return false;

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;

            try
            {
                repo.Update(plan);
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
