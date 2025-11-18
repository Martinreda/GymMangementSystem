// SelectTrainerViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace GymManagmentBSL.ViewModels.SessionViewModel
{
    public class SelectTrainerViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; } = null!;

        [Display(Name = "Specialization")]
        public string Specialization { get; set; } = null!;

        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}