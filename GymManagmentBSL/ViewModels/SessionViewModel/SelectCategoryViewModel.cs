// SelectCategoryViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace GymManagmentBSL.ViewModels.SessionViewModel
{
    public class SelectCategoryViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}