using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBClasses
{
    class OrderDB
    {
        // insert into row
        public bool Add(Orders order)
        {
            bool result = false;
            using (var db = new LibData())
            {
                db.Orders.Add(order);
                if (db.SaveChanges() != 0) result = true;
                else result = false;
            }
            if (result == true) return true;
            return false;
        }
        // Select all rows
        public List<Orders> SelectAll()
        {
            List<Orders> orders = new List<Orders>();
            using (var db = new LibData())
            {
                orders = db.Orders.ToList();
            }
            return orders;
        }
        // Select one row by id
        public Orders Select(int id)
        {
            Orders order;
            using (var db = new LibData())
            {
                order = db.Orders.Where(a => a.id == id).FirstOrDefault();
            }

            return order;
        }
    }
}
