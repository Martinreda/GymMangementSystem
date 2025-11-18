using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBSL.ViewModels.MemberViewModels
{
    public class MemberToUpdateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; 
        public string? Photo { get; set; }

        [Required(ErrorMessage = "Email Is Requierd")]
        [EmailAddress(ErrorMessage = "Invalid email Format ")] //Valdition
        [DataType(DataType.EmailAddress)] // UI Hint
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email Must Be Between 5 and 100 Characters")]
        public string Email { get; set; } = null!;




        [Required(ErrorMessage = "Phone Is Required")]
        [Phone(ErrorMessage = "Invalid Phone Format")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone Number Must Be Valid Egyptian PhoneNumber")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Building Number Is Required")]
        [Range(1, 9000, ErrorMessage = "Building Number Must Be Between 1 and 9000")]
        public int BuildingNumber { get; set; }


        [Required(ErrorMessage = "Street Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street must be between 2 and 30 Chars  ")]
        public string Street { get; set; } = null!;


        [Required(ErrorMessage = "City Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street must be between 2 and 30 Chars  ")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can contain only letters ans spaces")]
        public string City { get; set; } = null!;
    }
}
