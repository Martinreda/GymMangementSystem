using GymManagementDAL.Entities;
using GymManagmentBSL.Services.Interfaces;
using GymManagmentBSL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymMangementSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        #region Get All Plans
        public ActionResult Index()
        {
            var plans = _planService.GetAllPlans();
            return View(model: plans);
        }
        #endregion

        #region Get Plan Data
        public ActionResult PlanDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan ID.";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planService.GetPlanById(id);

            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Plan details loaded successfully.";
            return View(model: plan);
        }
        #endregion

        

        #region Edit Plan
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Plan ID cannot be zero or negative";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planService.GetPlanToUpdate(id);

            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found";
                return RedirectToAction(nameof(Index));
            }

            LoadPlanTypes();
            return View(model: plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UpdatePlanViewModel planToEdit)
        {
            if (!ModelState.IsValid)
            {
                LoadPlanTypes();
                return View(model: planToEdit);
            }

            var result = _planService.UpdatePlan(id, planToEdit);

            if (result)
            {
                TempData["SuccessMessage"] = "Plan updated successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update plan";
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

     

        #region Helper Methods
        private void LoadPlanTypes()
        {
            ViewBag.PlanTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "Basic" },
                new SelectListItem { Value = "1", Text = "Standard" },
                new SelectListItem { Value = "2", Text = "Premium" },
                new SelectListItem { Value = "3", Text = "VIP" }
            };
        }
        #endregion
    }
}