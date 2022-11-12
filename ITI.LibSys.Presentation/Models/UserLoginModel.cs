using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ITI.LibSys.Presentation.Models
{
    public class UserLoginModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
