using Microsoft.AspNetCore.Mvc;

namespace GymMangementSystem.Controllers
{
    public class MemberControllers : Controller
    {
        public IActionResult Index(int id)
        {
            return RedirectToRoute("Trainer", new {action ="GetTrainers"});
        }
       

        public ActionResult GetMembers(string name)
        {
            return View();
        }

        public ActionResult CreateMember()
        {
            return View();
        }
    }
}
