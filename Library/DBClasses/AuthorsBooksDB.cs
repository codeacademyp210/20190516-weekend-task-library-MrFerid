using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBClasses
{
    class AuthorsBooksDB
    {
        // insert into row
        public bool Add(AuthorsBooks authorBook)
        {
            bool result = false;
            using (var db = new LibData())
            {
                db.AuthorsBooks.Add(authorBook);
                if (db.SaveChanges() != 0) result = true;
                else result = false;
            }
            if (result == true) return true;
            return false;
        }
        // Select all rows
        public List<AuthorsBooks> SelectAll()
        {
            List<AuthorsBooks> authorsBooks = new List<AuthorsBooks>();
            using (var db = new LibData())
            {
                authorsBooks = db.AuthorsBooks.ToList();
            }
            return authorsBooks;
        }
        // Select rows by id
        public List<AuthorsBooks> Select(int bookID)
        {
            List<AuthorsBooks> authorsBooksList = new List<AuthorsBooks>();
            using (var db = new LibData())
            {
                authorsBooksList = db.AuthorsBooks.Where(a => a.BookID == bookID).ToList();
            }

            return authorsBooksList;
        }
        // Select by Author id property
        public List<AuthorsBooks> GetPairByAuthor(int authorID)
        {
            List<AuthorsBooks> abList = new List<AuthorsBooks>();

            using (var db = new LibData())
            {
                abList = db.AuthorsBooks.Where(c => c.AuthorID == authorID).ToList();
            }
            return abList;
        }

        // Update row
        public bool Update(AuthorsBooks authorBook, int bookId)
        {
            bool result = false;
            using (var db = new LibData())
            {
                var selectedAuthorBook = db.AuthorsBooks.Where(s => s.BookID == bookId).FirstOrDefault();

                selectedAuthorBook.AuthorID = authorBook.AuthorID;
                selectedAuthorBook.BookID = authorBook.BookID;

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
        public bool Delete(AuthorsBooks authorBook)
        {
            bool result = false;
            using (var db = new LibData())
            {
                db.AuthorsBooks.Attach(authorBook);
                db.AuthorsBooks.Remove(authorBook);
                if (db.SaveChanges() != 0) { result = true; }
                else result = false;
            }
            if (result == true) return true;
            return false;
        }
        // Delete multiple rows by bookID
        public bool DeleteRows(int bookID)
        {
            bool result;
            using(var db = new LibData())
            {
                db.AuthorsBooks.RemoveRange(db.AuthorsBooks.Where(s => s.BookID == bookID));

                if (db.SaveChanges() != 0)
                {
                    result = true;
                }
                else result = false;
            }
            if (result == true) return true;
            else return false;
        }
    }
}
