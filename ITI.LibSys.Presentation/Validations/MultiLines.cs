using System.ComponentModel.DataAnnotations;

namespace ITI.LibSys.Presentation.Validations
{
    public class MultiLines : ValidationAttribute
    {
        public int RowsCount { get; set; } = 1;
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string[] lines = value.ToString().Split(Environment.NewLine);
            if (lines.Length < RowsCount)
            {
                return new ValidationResult($"Rows must be more or equal than {RowsCount}");
            }
            return ValidationResult.Success;
        }
    }
}
