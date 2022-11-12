using ITI.LibSys.Models.Data;
using ITI.LibSys.Models;
using Microsoft.AspNetCore.Mvc;
using ITI.LibSys.Presentation.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using ITI.LibSys.Presentation.Filteration;

namespace ITI.LibSys.Presentation.Controllers.BookController
{
    [Authorize]
    public class BookController : Controller
    {
        private Context context;
        private IConfiguration config;
        public BookController(Context _context, IConfiguration _config)
        {
            context = _context;
            config= _config;
        }

        #region New Version
        #region Book Index
        [Authorize(Roles = "Viewer, Editor")]
        [LogFilteration]
        public IActionResult Index(int pageIndex=1, int pageSize=5)
        {
            ViewBag.Title = "Book Lists";
            var books = context.Books.ToPagedList(pageIndex, pageSize);
            return View(books);
        }
        [HttpGet]
        public IActionResult PagedBook(int pageIndex = 1, int pageSize= 5)
        {
            var pagedBooks=context.Books.ToPagedList(pageIndex, pageSize);
            return PartialView("_PagedBook", pagedBooks);
        }
        #endregion

        #region Create New Book
        [HttpGet]
        [Authorize(Roles ="Editor")]
        public IActionResult Create()
        {
            ViewBag.Authors = context.Authors
                .Select(a => new SelectListItem(a.Name, a.ID.ToString()));
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookModel model)
        {
            List<BookImage> Images = new List<BookImage>();
            if (ModelState.IsValid == false)
            {
                ViewBag.Authors = context.Authors
                    .Select(a => new SelectListItem(a.Name, a.ID.ToString()));
                //ViewBag.Done = false;
                return View();
            }
            else
            {
                //to load file form PC to the App
                foreach (IFormFile file in model.Images)
                {
                    string FileName = Guid.NewGuid().ToString() + file.FileName;//to add file without any errors
                    Images.Add(new BookImage { Path = FileName });//for insert the file name in the BookImages Table
                    FileStream f = new FileStream(
                        Path.Combine(Directory.GetCurrentDirectory(), "Content", "Images", FileName),
                        FileMode.OpenOrCreate, FileAccess.ReadWrite);//to load file in stream
                    file.CopyTo(f);//to copy stream in the file
                    f.Position = 0;//to tell stream that it finished
                }
                context.Books.Add(new Book
                {
                    Title = model.Title,
                    Discription = model.Description,
                    AuthorID = model.AuthorID,
                    BookImages = Images
                });
                context.SaveChanges();
                //ViewBag.Done = true;
                return View();
            }
        }
        #endregion

        #region Book Details
        [HttpGet]
        [Authorize(Roles = "Viewer, Editor")]
        public IActionResult Details(int id)
        {
            ViewBag.Title = "Book-Details";
            var book = context.Books.FirstOrDefault(b => b.ID == id);
            ViewBag.Images = config.GetSection("Images").Value.ToString();
            return View(book);
        }
        #endregion

        #region Book Edit
        [HttpGet]
        [Authorize(Roles ="Editor")]
        public IActionResult Edit(int id)
        {
            ViewBag.Authors = context.Authors
                .Select(a => new SelectListItem(a.Name, a.ID.ToString()));
            var book = context.Books.FirstOrDefault(b => b.ID == id);
            var bookModel = new BookModel
            {
                Id = book.ID,
                Title = book.Title,
                AuthorID = book.AuthorID,
                Description = book.Discription,
            };
            return View(bookModel);
        }
        [HttpPost]
        public IActionResult Edit(BookModel model)
        {
            List<BookImage> Images = new List<BookImage>();
            //to load file form PC to the App
            foreach (IFormFile file in model.Images)
            {
                string FileName = Guid.NewGuid().ToString() + file.FileName;//to add file without any errors
                Images.Add(new BookImage { Path = FileName });//for insert the file name in the BookImages Table
                FileStream f = new FileStream(
                    Path.Combine(Directory.GetCurrentDirectory(), "Content", "Images", FileName),
                    FileMode.OpenOrCreate, FileAccess.ReadWrite);//to load file in stream
                file.CopyTo(f);//to copy stream in the file
                f.Position = 0;//to tell stream that it finished
            }
            Book book = context.Books.FirstOrDefault(b => b.ID == model.Id);
            book.Title = model.Title;
            book.AuthorID = model.AuthorID;
            book.Discription = model.Description;
            book.BookImages = Images;
            context.Books.Update(book);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete Book
        [HttpGet]
        [Authorize(Roles ="Editor")]
        public IActionResult Delete(int id)
        {
            var book = context.Books.FirstOrDefault(b => b.ID == id);
            return View(book);
        }
        [HttpPost]
        public IActionResult Delete(Book book)
        {
            context.Books.Remove(book);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion 
        #endregion

        #region Old Version
        //#region Book Index
        //public IActionResult Index()
        //{
        //    ViewBag.Title = "Book Lists";
        //    List<Book> books = new List<Book>();
        //    books = context.Books.ToList();
        //    return View(books);
        //}
        //#endregion

        //#region Book Details
        //public IActionResult Details(int id)
        //{
        //    ViewBag.Title = "Book-Details";
        //    var book = context.Books.FirstOrDefault(b => b.ID == id);
        //    return View(book);
        //}
        //#endregion

        //#region Create Book
        //public ActionResult Create()
        //{
        //    List<Author> authors = new List<Author>();
        //    authors = context.Authors.ToList();
        //    return View(authors);
        //}
        //public ActionResult SaveBook()
        //{
        //    string title = Request.Form["bookTitle"];
        //    int author = int.Parse(Request.Form["bookAuthor"].ToString());
        //    string disc = Request.Form["bookDisc"];
        //    if (title != null && author != 0)
        //    {
        //        Book book = new Book();
        //        book.Title = title;
        //        book.AuthorID = author;
        //        book.Discription = disc;
        //        context.Books.Add(book);
        //        context.SaveChanges();
        //    }
        //    return Redirect("Index");
        //}
        //#endregion

        //#region Edit Book
        //public IActionResult Edit(int id)
        //{
        //    var book = context.Books.FirstOrDefault(b => b.ID == id);
        //    return View(book);
        //}

        //public IActionResult EditBook()
        //{
        //    int id = int.Parse(Request.Form["bookId"].ToString());
        //    string title = Request.Form["bookTitle"].ToString();
        //    string author = Request.Form["bookAuthor"].ToString();
        //    Book book = context.Books.FirstOrDefault(b => b.ID == id);
        //    if (title != null && author != null)
        //    {
        //        book.Title = title;
        //        book.Author.Name = author;
        //        context.Books.Update(book);
        //        context.SaveChanges();
        //    }
        //    return Redirect("Index");
        //}
        //#endregion

        //#region Delete Book
        //public IActionResult Delete(int id)
        //{
        //    var book = context.Books.FirstOrDefault(b => b.ID == id);
        //    return View(book);
        //}

        //public IActionResult DeleteBook()
        //{
        //    int id = int.Parse(Request.Form["bookId"].ToString());
        //    Book book = context.Books.FirstOrDefault(b => b.ID == id);
        //    context.Books.Remove(book);
        //    context.SaveChanges();
        //    return Redirect("Index");
        //}
        //#endregion 
        #endregion

    }
}
