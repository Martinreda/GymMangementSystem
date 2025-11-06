using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBSL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {
       
        public string PlanName { get; set; } = null!;

        [Required(ErrorMessage = "Plan Description Id Required")]
        [StringLength(200 , MinimumLength = 5 ,  ErrorMessage = ("Plan Description must be less than 201 chars"))]
        public string Description { get; set; } = null!;

        [Required (ErrorMessage = "Duration Days Is Rewuired")]
        [Range (minimum: 1 , maximum: 365 , ErrorMessage = "Duration Days must be between 1 and 365" )]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price Is Rewuired")]
        [Range(minimum: 1, maximum: 10000, ErrorMessage = "price must be between 1 and 10000")]
        public decimal Price { get; set; }
    }
}
