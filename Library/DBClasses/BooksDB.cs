using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBClasses
{
    class BooksDB
    {
        // insert into row
        public int Add(Books book)
        {
            int lastID;
            using (var db = new LibData())
            {
                db.Books.Add(book);
                if (db.SaveChanges() != 0)
                {
                    lastID = book.id;
                }
                else
                {
                    lastID = -1;
                }
            }
            return lastID;
        }
        // Select all rows
        public List<Books> SelectAll()
        {
            List<Books> bookList = new List<Books>();
            using (var db = new LibData())
            {
                bookList = db.Books.ToList();
            }
            return bookList;
        }
        // Select one row by id
        public Books Select(int bookID)
        {
            Books myBook;
            using (var db = new LibData())
            {
                myBook = db.Books.Where(a => a.id == bookID).FirstOrDefault();
            }

            return myBook;
        }

        // Select book By category
        public List<Books> GetBooksByCategory(int categoryID)
        {
            List<Books> bookList = new List<Books>();

            using (var db = new LibData())
            {
                bookList = db.Books.Where(a => a.CategoryID == categoryID).ToList();
            }

            return bookList;
        }

        // Select by Name or Cathegory property
        public List<Books> GetBook(string prop)
        {
            List<Books> bookList = new List<Books>();

            using (var db = new LibData())
            {
                bookList = db.Books.Where(c => c.Name.ToLower() == prop.ToLower()).ToList();
            }
            return bookList;
        }
        // Update row
        public bool Update(Books book, int oldID)
        {
            bool result = false;
            using (var db = new LibData())
            {
                var selectedBook = db.Books.Find(oldID);

                selectedBook.Name = book.Name;
                selectedBook.CategoryID = book.CategoryID;
                selectedBook.Count = book.Count;
                selectedBook.Price = book.Price;

                if (db.SaveChanges() != 0)
                {
                    result = true;
                }
                else result = false;
            }
            if (result == true) return true;
            return false;
        }
        // Delete row
        public bool Delete(Books book)
        {
            bool result = false;
            using (var db = new LibData())
            {
                db.Books.Attach(book);
                db.Books.Remove(book);
                if (db.SaveChanges() != 0) { result = true; }
                else result = false;
            }
            if (result == true) return true;
            return false;
        }
    }
}
