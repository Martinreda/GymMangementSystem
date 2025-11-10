using GymManagmentBSL.Services.Interfaces;
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
        // BaseUrl/Member/MemberDetails -> id = 0
        // BaseUrl/Member/MemberDetails/1 -> id = 1

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
        #endregion
    }
}
