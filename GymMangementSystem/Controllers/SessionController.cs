using GymManagmentBSL.Services.Interfaces;
using GymManagmentBSL.ViewModels.SessionViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GymMangementSystem.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        #region Get All Sessions
        public ActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions();
            return View(model: sessions);
        }
        #endregion

        #region Get Session Data
        public ActionResult SessionDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session ID.";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionById(id);

            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Session details loaded successfully.";
            return View(model: session);
        }
        #endregion

        #region Create Session
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateSessionViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Please correct the errors in the form.";
                    return View(model: model);
                }

                var result = _sessionService.CreateSession(model);

                if (result)
                {
                    TempData["SuccessMessage"] = "Session created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create session. Please check the provided data.";
                    return View(model: model);
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while creating the session.";
                return View(model: model);
            }
        }
        #endregion

        #region Edit Session
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Session ID cannot be zero or negative";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionToUpdate(id);

            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found or cannot be updated";
                return RedirectToAction(nameof(Index));
            }

            return View(model: session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UpdateSessionViewModel sessionToEdit)
        {
            if (!ModelState.IsValid)
            {
                return View(model: sessionToEdit);
            }

            var result = _sessionService.UpdateSession(sessionToEdit, id);

            if (result)
            {
                TempData["SuccessMessage"] = "Session updated successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update session";
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Session
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Session ID cannot be zero or negative";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionById(id);

            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found";
                return RedirectToAction(nameof(Index));
            }

            // Check if session can be deleted
            bool canDelete = _sessionService.CanSessionBeDeleted(id);
            ViewBag.CanDelete = canDelete;

            if (!canDelete)
            {
                ViewBag.DeleteReason = GetDeleteReason(session);
            }

            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([FromRoute] int id)
        {
            var session = _sessionService.GetSessionById(id);

            if (session == null)
            {
                TempData["ErrorMessage"] = "Session not found";
                return RedirectToAction(nameof(Index));
            }

            if (!_sessionService.CanSessionBeDeleted(id))
            {
                TempData["ErrorMessage"] = "Cannot delete session. " + GetDeleteReason(session);
                return RedirectToAction(nameof(Delete), new { id });
            }

            var result = _sessionService.RemoveSession(id);

            TempData[result ? "SuccessMessage" : "ErrorMessage"] =
                result ? "Session deleted successfully" : "Failed to delete session";

            return RedirectToAction(nameof(Index));
        }

        private string GetDeleteReason(SessionViewModel session)
        {
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now)
                return "Session is currently ongoing.";

            if (session.StartDate > DateTime.Now)
                return "Session is upcoming and cannot be deleted.";

            if (session.AvailableSlots < session.Capacity)
                return "Session has active bookings.";

            return "Session cannot be deleted at this time.";
        }
        #endregion
    }
}