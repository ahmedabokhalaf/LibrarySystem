using ITI.LibSys.Presentation.Validations;
using System.ComponentModel.DataAnnotations;

namespace ITI.LibSys.Presentation.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        [
            Required(ErrorMessage = "Title Field is Required"),
            MaxLength(100, ErrorMessage = "Title musn't be more than 100 Characters"),
            MinLength(3, ErrorMessage = "Title musn't be less than 3 Characters"),
            Display(Name = "Book Title")
        ]
        public string Title { get; set; }

        [
            Required(ErrorMessage = "Description Field is Required"),
            Display(Name = "Book Description"),
            MultiLines(RowsCount = 3)
        ]
        public string Description { get; set; }

        [
            Required(ErrorMessage = "You must Select Auhtor Name"),
            Display(Name = "Book Author Name"),
        ]
        public int AuthorID { get; set; }

        [
            Required(ErrorMessage = "You must minimum upload one Image"),
            Display(Name = "Book Images")
        ]
        [OnlyImages(new string[] {".jpg",".png"})]
        public List<IFormFile> Images { get; set; }
    }
}
