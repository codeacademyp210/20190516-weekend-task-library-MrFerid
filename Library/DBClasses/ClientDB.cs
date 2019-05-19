using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBClasses
{
    class ClientDB
    {
        // insert into row
        public bool Add(Clients client)
        {
            bool result = false;
            using (var db = new LibData())
            {
                db.Clients.Add(client);
                if (db.SaveChanges() != 0) result = true;
                else result = false;
            }
            if (result == true) return true;
            return false;
        }
        // Select all rows
        public List<Clients> SelectAll()
        {
            List<Clients> clients = new List<Clients>();
            using (var db = new LibData())
            {
                clients = db.Clients.ToList();
            }
            return clients;
        }
        // Select one row by id
        public Clients Select(int id)
        {
            Clients client;
            using (var db = new LibData())
            {
                client = db.Clients.Where(a => a.id == id).FirstOrDefault();
            }

            return client;
        }
        // Get row by any property
        public List<Clients> GetClient(string prop)
        {
            List<Clients> clientList = new List<Clients>();

            using(var db = new LibData())
            {
                 clientList =  db.Clients.Where(c => c.Name.ToLower() == prop.ToLower()).ToList();
                if(clientList.Count == 0)
                {
                    clientList = db.Clients.Where(c => c.Surname.ToLower() == prop.ToLower()).ToList();
                }
                if (clientList.Count == 0)
                {
                    clientList = db.Clients.Where(c => c.Phone.ToLower() == prop.ToLower()).ToList();
                }
                if (clientList.Count == 0)
                {
                    clientList = db.Clients.Where(c => c.Email.ToLower() == prop.ToLower()).ToList();
                }
            }
            return clientList;
        }

        // Update row
        public bool Update(Clients client, int oldID)
        {
            bool result = false;
            using (var db = new LibData())
            {
                var selectedClient = db.Clients.Find(oldID);

                selectedClient.Name = client.Name;
                selectedClient.Surname = client.Surname;

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
        public bool Delete(Clients client)
        {
            bool result = false;
            using (var db = new LibData())
            {
                db.Clients.Attach(client);
                db.Clients.Remove(client);
                if (db.SaveChanges() != 0) { result = true; }
                else result = false;
            }
            if (result == true) return true;
            return false;
        }
    }
}
