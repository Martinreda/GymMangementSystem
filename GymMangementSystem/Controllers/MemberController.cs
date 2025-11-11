using GymManagementDAL.Entities.Enums;
using GymManagmentBSL.Services.Interfaces;
using GymManagmentBSL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymMangementSystem.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        #region Get All Members
        public ActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(model: members);
        }


        #endregion

        #region Get Member Data
        public ActionResult MemberDetails(int id)
        {
            // 1. Validate ID input
            if (id <= 0)
            {
                return RedirectToAction(actionName: nameof(Index));
            }

            // 2. Retrieve data via service layer
            var Member = _memberService.GetMemberDetails(id);

            // 3. Handle data not found
            if (Member is null)
            {
                return RedirectToAction(actionName: nameof(Index));
            }

            // 4. Return view with model
            return View(model: Member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction(actionName: nameof(Index));
            }

            var HealthRecord = _memberService.GetMemberHealthRecordDetails( id);

            if (HealthRecord is null) return RedirectToAction(actionName: nameof(Index));

            return View(model: HealthRecord);
        }
        #endregion

        #region Create Member
        // GET: Member/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Member/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateMemberViewModel model)
        {
            try
            {
                // 1. Validate model state
                if (!ModelState.IsValid)
                {
                    return View(model: model);
                }

                // 2. Call service to create member
                var result = _memberService.CreateMember(model);

                // 3. Handle result
                if (result)
                {
                    // Success - redirect to members list
                    return RedirectToAction(actionName: nameof(Index));
                }
                else
                {
                    // Failure - show error message
                    ModelState.AddModelError("", "Failed to create member. Email or phone may already exist.");
                    return View(model: model);
                }
            }
            catch (System.Exception)
            {
                // Handle unexpected errors
                ModelState.AddModelError("", "An unexpected error occurred while creating the member.");
                return View(model: model);
            }
        }
        #endregion
        // في MemberController.cs
        public IActionResult AddTestMember()
        {
            var result = _memberService.CreateMember(new CreateMemberViewModel
            {
                Name = "Test Member",
                Email = "test@test.com",
                Phone = "01012345678",
                DateOfBirth = new DateOnly(1990, 1, 1),
                Gender = Gender.Male,
                BuildingNumber = 123,
                Street = "Test Street",
                City = "Test City",
                HealthRecordViewModel = new HealthRecordViewModel
                {
                    Height = 175.0m,
                    Weight = 70.0m,
                    BloodType = "A+",
                    Note = "Test record"
                }
            });

            if (result)
                TempData["Success"] = "Member added!";
            else
                TempData["Error"] = "Failed to add member";

            return RedirectToAction("Index");
        }
    }
}