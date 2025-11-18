using GymManagementDAL.Entities.Enums;
using GymManagmentBSL.Services.Interfaces;
using GymManagmentBSL.ViewModels.TrianerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymMangementSystem.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;  

        public TrainerController(ITrainerService trainerService)  
        {
            _trainerService = trainerService;
        }

        // باقي الكود كما هو...
        #region Get All Trainers
        public ActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(model: trainers);
        }
        #endregion

        #region Get Trainer Data
        public ActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer ID.";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerDetails(id);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Trainer details loaded successfully.";
            return View(model: trainer);
        }
        #endregion

        #region Create Trainer
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTrainerViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Please correct the errors in the form.";
                    return View(model: model);
                }

                var result = _trainerService.CreateTrainer(model);

                if (result)
                {
                    TempData["SuccessMessage"] = "Trainer created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create trainer. Email or phone may already exist.";
                    return View(model: model);
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while creating the trainer.";
                return View(model: model);
            }
        }
        #endregion

        #region Edit Trainer
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Trainer ID cannot be zero or negative";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerToUpdate(id);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }

            return View(model: trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TrainerToUpdateViewModel trainerToEdit, IFormFile photoFile)
        {
            if (!ModelState.IsValid)
            {
                return View(model: trainerToEdit);
            }

            if (photoFile != null && photoFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(photoFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("PhotoFile", "Only JPG, PNG, and GIF files are allowed");
                    return View(model: trainerToEdit);
                }

                if (photoFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("PhotoFile", "File size must be less than 5MB");
                    return View(model: trainerToEdit);
                }

                try
                {
                    var fileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine("wwwroot", "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        photoFile.CopyTo(stream);
                    }

                    trainerToEdit.Photo = fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PhotoFile", "Error uploading file: " + ex.Message);
                    return View(model: trainerToEdit);
                }
            }

            var result = _trainerService.UpdateTrainerDetails(trainerToEdit, id);

            if (result)
            {
                TempData["SuccessMessage"] = "Trainer updated successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update trainer";
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Trainer
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Trainer ID cannot be zero or negative";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerDetails(id);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }

            bool hasActiveSessions = _trainerService.HasActiveSessions(id);
            ViewBag.HasActiveSessions = hasActiveSessions;

            if (hasActiveSessions)
            {
                var activeSessions = _trainerService.GetActiveSessions(id);
                ViewBag.ActiveSessions = activeSessions;
            }

            return View(trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([FromRoute] int id)
        {
            bool hasActiveSessions = _trainerService.HasActiveSessions(id);

            if (hasActiveSessions)
            {
                TempData["ErrorMessage"] = "Cannot delete trainer with active sessions. Please cancel all active sessions first.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            var result = _trainerService.RemoveTrainer(id);

            TempData[result ? "SuccessMessage" : "ErrorMessage"] =
                result ? "Trainer deleted successfully" : "Failed to delete trainer";

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}