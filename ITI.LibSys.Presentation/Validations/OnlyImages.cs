using System.ComponentModel.DataAnnotations;

namespace ITI.LibSys.Presentation.Validations
{
    public class OnlyImages : ValidationAttribute
    {
        public string[] Extensions { get; set; }
        public OnlyImages(string[] extensions)
        {
            Extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var Image = value as List<IFormFile>;
            if (Image != null)
            {
                foreach(var file in Image)
                {
                    var ImagePath = Path.GetExtension(file.FileName);
                    if (!Extensions.Contains(ImagePath.ToLower()))
                    {
                        return new ValidationResult("Image extension is not Valid");
                    }
                }
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("You must minimum upload one Image");
            }
        }
    }
}
