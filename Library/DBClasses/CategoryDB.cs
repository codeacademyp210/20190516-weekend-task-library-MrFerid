using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBClasses
{
    class CategoryDB
    {
        // insert into row
        public bool Add(Categories category)
        {
            bool result = false;
            using (var db = new LibData())
            {
                db.Categories.Add(category);
                if (db.SaveChanges() != 0) result = true;
                else result = false;
            }
            if (result == true) return true;
            return false;
        }
        // Select all rows
        public List<Categories> SelectAll()
        {
            List<Categories> categories = new List<Categories>();
            using (var db = new LibData())
            {
                categories = db.Categories.ToList();
            }
            return categories;
        }
        // Select one row by id
        public Categories Select(int id)
        {
            Categories categories;
            using (var db = new LibData())
            {
                categories = db.Categories.Where(a => a.id == id).FirstOrDefault();
            }

            return categories;
        }
        // Select by Name or Cathegory property
        public List<Categories> GetCategory(string prop)
        {
            List<Categories> categoryList = new List<Categories>();

            using (var db = new LibData())
            {
                categoryList = db.Categories.Where(c => c.Name.ToLower() == prop.ToLower()).ToList();
            }
            return categoryList;
        }
        // Update row
        public bool Update(Categories category, int oldID)
        {
            bool result = false;
            using (var db = new LibData())
            {
                var selectedCategory = db.Categories.Find(oldID);

                selectedCategory.Name = category.Name;

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
        public bool Delete(Categories category)
        {
            bool result = false;
            using (var db = new LibData())
            {
                db.Categories.Attach(category);
                db.Categories.Remove(category);
                if (db.SaveChanges() != 0) { result = true; }
                else result = false;
            }
            if (result == true) return true;
            return false;
        }
    }
}
