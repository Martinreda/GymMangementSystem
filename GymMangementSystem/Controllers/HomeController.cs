using GymManagementDAL.Entities;
using GymManagmentBSL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymMangementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public HomeController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        public ActionResult Index()
        {
            var Data = _analyticsService.GetAnalyticsData();
            return View(model: Data);
        }
        
        public JsonResult Trainers()
        {
            var Trainers = new List<Trainer>()
        {
            new Trainer() { Name = "Mohamed", Phone = "12345678" },
            new Trainer() { Name = "Amr", Phone = "812355" }
        };

            return Json(data: Trainers);
        }
        public RedirectResult Redirect()
        {
            return Redirect("https://github.com/Martinreda/GymMangementSystem/tree/Demo03");
        }
        public ContentResult Content()
        {
            return Content("Hello rom GMS");
        }

        public FileResult DownloadFile()
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "css", "site.css");
            var FileBytes = System.IO.File.ReadAllBytes(path: FilePath);

            return File(fileContents: FileBytes, contentType: "text/css", fileDownloadName: "DownloadableSite.css");
        }

        public EmptyResult EmptyResult()
        {
            return new EmptyResult();
        }
    }
}
