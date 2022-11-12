using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ITI.LibSys.Presentation.Models
{
    public class UserRegisterModel
    {
        [Required, MaxLength(100), MinLength(3), Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, MaxLength(100), MinLength(3), Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, MaxLength(20), MinLength(3), Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required, EmailAddress, Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required, MinLength(8), MaxLength(32)]
        [Display(Name = "Password"), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, Compare("Password")]
        [Display(Name = "Confirm Password"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Display(Name ="User Role")]
        public string Role { get; set; }
    }
}
