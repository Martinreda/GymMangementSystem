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
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member ID.";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberDetails(id);

            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Member details loaded successfully.";
            return View(model: member);
        }


        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member ID.";
                return RedirectToAction(nameof(Index));
            }

            var healthRecord = _memberService.GetMemberHealthRecordDetails(id);

            if (healthRecord == null)
            {
                TempData["ErrorMessage"] = "No Health Record found for this member.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Health Record loaded successfully.";
            return View(healthRecord);
        }
        #endregion


        #region Create Member

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateMemberViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Please correct the errors in the form.";
                    return View(model: model);
                }

                var result = _memberService.CreateMember(model);

                if (result)
                {
                    TempData["SuccessMessage"] = "Member created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create member. Email or phone may already exist.";
                    return View(model: model);
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while creating the member.";
                return View(model: model);
            }
        }

        #endregion
        #region Edit
        public ActionResult Edit(int id)  // تغيير الاسم من MemberEdit إلى Edit
        {
            // 1. Input Validation (ID Check)
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Member ID cannot be zero or negative";
                return RedirectToAction(nameof(Index));
            }

            // 2. Retrieve data via service layer
            var member = _memberService.GetMemberToUpdate(id);

            // 3. Handle data not found
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }

            // 4. Return view with model
            return View(model: member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, MemberToUpdateViewModel memberToEdit, IFormFile photoFile)  // تغيير الاسم هنا أيضاً
        {
            // 1. Check client-side validation results
            if (!ModelState.IsValid)
            {
                return View(model: memberToEdit);
            }

            // 2. Handle file upload if provided
            if (photoFile != null && photoFile.Length > 0)
            {
                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(photoFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("PhotoFile", "Only JPG, PNG, and GIF files are allowed");
                    return View(model: memberToEdit);
                }

                // Validate file size (5MB max)
                if (photoFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("PhotoFile", "File size must be less than 5MB");
                    return View(model: memberToEdit);
                }

                // Process file upload
                try
                {
                    var fileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine("wwwroot", "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        photoFile.CopyTo(stream);
                    }

                    memberToEdit.Photo = fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PhotoFile", "Error uploading file: " + ex.Message);
                    return View(model: memberToEdit);
                }
            }

            // 3. Delegate update logic to the service layer
            var result = _memberService.UpdateMemberDetails(id, memberToEdit);

            // 4. Handle service layer result
            if (result)
            {
                TempData["SuccessMessage"] = "Member updated successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update member";
            }

            // 5. Redirect after setting TempData
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Delete Member
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberDetails(id);

            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            bool hasActiveSessions = _memberService.HasFutureSessions(id);
            ViewBag.HasActiveSessions = hasActiveSessions;

            if (hasActiveSessions)
                ViewBag.ActiveSessions = _memberService.GetFutureSessions(id);

            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([FromRoute] int id)
        {
            bool hasActiveSessions = _memberService.HasFutureSessions(id);

            if (hasActiveSessions)
            {
                TempData["ErrorMessage"] = "Cannot delete member with active sessions. Please cancel all active sessions first.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            var result = _memberService.RemoveMember(id);

            TempData[result ? "SuccessMessage" : "ErrorMessage"] =
                result ? "Member Deleted Successfully" : "Member Can Not Be Deleted";

            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
