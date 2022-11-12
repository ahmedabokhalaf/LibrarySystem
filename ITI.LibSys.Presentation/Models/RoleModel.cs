using System.ComponentModel.DataAnnotations;

namespace ITI.LibSys.Presentation.Models
{
    public class RoleModel
    {
        [Required]
        [MaxLength(100), MinLength(3)]
        [Display(Name="Role Name")]
        public string Name { get; set; }
    }
}
