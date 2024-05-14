using Book2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Book2.Controllers
{
    public class Book2Controller : Controller
    {
        Book2Context context = new Book2Context();


        //General Reponse
        public class Response
        {
            public int Status { get; set; }
            public string Message { get; set; }
        }
        public class BookVM
        {
            public int id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string publishDate { get; set; }
            public string autherName { get; set; }
            public int? autherId { get; set; }
        }

        #region GetAutherName

        public IActionResult GetAutherName(int id)
        {
            var auth = context.Authers.Where(e => e.Id == id).SingleOrDefault();
            return Json(auth);
        }

        #endregion

        #region GetAuthers 

        public IActionResult GetAuthers()
        {
            List<Auther> authers = context.Authers.ToList();
            return Json(authers);
        }

        #endregion

        #region DeleteBook

        public IActionResult DeleteBook(int id)
        {
            List<Book> books = context.Books.ToList();
            var book = books.Where(e => e.Id == id).SingleOrDefault();
            book.IsDeleted = true;
            context.SaveChanges(); 
            return Json("Book Is Deleted"); 
        }

        #endregion

        #region Edit Book
        public IActionResult EditBook(int id)
        {
            var book = context.Books.Where(e => e.Id == id).SingleOrDefault();
            return Json(book);
        }
        #endregion

        #region UpdateBook
        [HttpPost]
        public IActionResult UpdateBook(Book b)
        {
            context.Books.Update(b);
            context.SaveChanges();
            return Json("Record Updated");
        }

        #endregion

        #region BookList
        public IActionResult BookList()
        {
            List<BookVM> book = context.Books.Include(b => b.Auther).Where(e => e.IsDeleted == null).OrderByDescending(book => book.Id).ToList().Select(book => new BookVM
            {
                id = book.Id,
                title = book.Title,
                autherName = book.Auther.Name,
                autherId = book.Auther.Id,
                publishDate = book.PublishDate,
                description = book.Description

            }).ToList();
            return Json(book);
        }
        #endregion

        #region SaveBook

        [HttpPost]
        public IActionResult saveBook(Book b)
        {
            //auto mapper

            //var book = new Book()
            //{
            //    Title = b.Title,
            //    Description = b.Description,
            //    PublishDate = b.PublishDate,
            //    AutherId = b.AutherId
            //};  
            if (b.Title == null)
                return Json(new Response { Status = 428, Message = "Title is Requered" });
                context.Books.Add(b);
                context.SaveChanges();
                return Json(new Response { Status = 200, Message = "Data Is Saved" });
        }

        #endregion

        #region DetailsUsingPartial 
        public IActionResult DetailsUsingPartial (int id)
        {
            List<Book> books = context.Books.ToList();
            var book = books.FirstOrDefault(e => e.Id == id);

            List<Auther> authers = context.Authers.ToList();
            var auther = authers.FirstOrDefault(e => e.Id == book.AutherId);
            ViewData["SelectedAuther"] = auther;


            return PartialView("_BookCardPartial", book); 
        }
        #endregion

        #region GetAll Action
        public IActionResult GetAll()
        {
            List<Book> books = context.Books.Where(e => e.IsDeleted == null).ToList();
            ViewData["AutherList"] = context.Authers.ToList();
            return View(books);
        }
        #endregion

        #region GetDetails Action 
        
        public IActionResult GetDetails(int id)
        {
           
            List<Book>books = context.Books.ToList();
            var book = books.FirstOrDefault(e => e.Id == id);

            // ViewData => auther
            List<Auther>authers = context.Authers.ToList();
            var auther = authers.FirstOrDefault(e => e.Id == book.AutherId);
            ViewData["SelectedAuther"] = auther;

            return View(book);  
        }

        #endregion

        #region AddNew Action 
        public IActionResult AddNew()
        {
            ViewData["AutherList"] = context.Authers.ToList();

            return View(new Book()); 
        }
        #endregion

        #region Save New Action
        public IActionResult SaveNew(Book B) 
        {
            if(B.Title !=null)
            {
                context.Books.Add(B); 
                context.SaveChanges();
                return RedirectToAction("GetAll");
            }
            else
            {
                ViewData["AutherList"] = context.Authers.ToList();
                return View("AddNew",B); 
            }

        }
        #endregion

        #region Edit Action 
        public IActionResult Edit(int id)
        {

            List<Book> books = context.Books.ToList();

            var book = books.FirstOrDefault(d => d.Id == id);
            ViewData["AutherList"] = context.Authers.ToList();

            return View(book);
        }
        #endregion

        #region Save Edit Action 
        public IActionResult SaveEdit(int id, Book p)
        {
            var oldBook = context.Books.FirstOrDefault(p => p.Id == id);
            if (p.Title != null)
            {
                if (oldBook != null)
                {
                    oldBook.Title = p.Title;
                    oldBook.Description = p.Description;
                    oldBook.PublishDate = p.PublishDate;
                    oldBook.AutherId = p.AutherId;
                    context.SaveChanges();
                    return RedirectToAction("GetAll");
                }
                else
                {
                    return View("Edit", p);
                }
            }
            else
            {
                ViewData["AutherList"] = context.Authers.ToList();

                return View("Edit", p);
            }
        }
        #endregion

        #region Delete Action
        public IActionResult Delete(int id)
        {
            List<Book> books = context.Books.ToList();
            var book = books.FirstOrDefault(e => e.Id == id); 
            if(book != null)
            {
                book.IsDeleted = true;
                List<Book> books1 = books.Where(e => e.IsDeleted == null).ToList();
                context.SaveChanges(); 
                return RedirectToAction("GetAll"); 
            }
            else
            {
                return RedirectToAction("GetAll"); 
            }
        }
        #endregion
    }
}
