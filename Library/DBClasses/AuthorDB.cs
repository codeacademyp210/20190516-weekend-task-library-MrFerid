using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBClasses
{
    class AuthorDB
    {
        // insert into row
        public bool Add(Authors author)
        {
            bool result = false;
            using (var db = new LibData())
            {
                db.Authors.Add(author);
                if (db.SaveChanges() != 0) result = true;
                else result = false;
            }
            if (result == true) return true;
            return false;
        }
        // Select all rows
        public List<Authors> SelectAll()
        {
            List<Authors> authors = new List<Authors>();
            using (var db = new LibData())
            {
                authors = db.Authors.ToList();
            }
            return authors;
        }
        // Select one row by id
        public Authors Select(int id)
        {
            Authors myAuthor;
            using (var db = new LibData())
            {
                myAuthor = db.Authors.Where(a => a.id == id).FirstOrDefault();
            }

            return myAuthor;
        }
        // Select by Name property
        public List<Authors> GetAuthor(string prop)
        {
            List<Authors> authorList = new List<Authors>();

            using (var db = new LibData())
            {
                authorList = db.Authors.Where(c => c.Name.ToLower() == prop.ToLower()).ToList();
            }
            return authorList;
        }

        // Update row
        public bool Update(Authors author, int oldID)
        {
            bool result = false;
            using(var db = new LibData())
            {
                var selectedAuthor = db.Authors.Find(oldID);

                selectedAuthor.Name = author.Name;
                selectedAuthor.Surname = author.Surname;

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
        public bool Delete(Authors author)
        {
            bool result = false;
            using(var db = new LibData())
            {
                db.Authors.Attach(author);
                db.Authors.Remove(author);
                if (db.SaveChanges() != 0) { result = true; }
                else result = false;
            }
            if (result == true) return true;
            return false;
        }
    }
}
