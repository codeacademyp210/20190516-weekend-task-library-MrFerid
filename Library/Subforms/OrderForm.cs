using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library.Subforms
{
    public partial class OrderForm : Form
    {
        DBClasses.ClientDB clients = new DBClasses.ClientDB();
        DBClasses.BooksDB books = new DBClasses.BooksDB();
        DBClasses.AuthorDB authors = new DBClasses.AuthorDB();
        DBClasses.AuthorsBooksDB authorsBooksDB = new DBClasses.AuthorsBooksDB();
        DBClasses.CategoryDB categories = new DBClasses.CategoryDB();
        DBClasses.OrderDB orders = new DBClasses.OrderDB();
        public OrderForm()
        {
            InitializeComponent();
            refreshGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SearchOrder();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            SearchOrder();
            if(txtName.Text == string.Empty)
            {
                refreshGrid();
            }
        }

        // refresh grid
        public void refreshGrid()
        {
            List<Orders> orderList;
            using (var db = new LibData())
            {
                orderList = db.Orders.ToList();
                dataGridView2.Rows.Clear();
                fillGrid(orderList);
            }
        }

        // Search for order
        public void SearchOrder()
        {
            string srcTxt = txtName.Text;
            List<Orders> orderList = new List<Orders>();
            List<Orders> orderList2 = new List<Orders>();

            List<Books> bookList = new List<Books>();
            using (var db = new LibData())
            {
                orderList = db.Orders.Where(o => o.Clients.Name.ToLower().Contains(srcTxt.ToLower()) || o.Clients.Surname.ToLower().Contains(srcTxt.ToLower())).ToList();
                bookList = db.AuthorsBooks.Where(ab => ab.Authors.Name.ToLower().Contains(srcTxt.ToLower()) || ab.Authors.Surname.ToLower().Contains(srcTxt.ToLower())).Select(b => b.Books).ToList();

                if (orderList.Count > 0)
                {
                    fillGrid(orderList);
                }
                if (bookList.Count > 0)
                {
                    for (int i = 0; i < bookList.Count; i++)
                    {
                        int bookid = bookList[i].id;
                        orderList2 = (db.Orders.Where(b => b.BookID == bookid)).ToList();

                    }
                }
                orderList.AddRange(orderList2);
                fillGrid(orderList);
            }
        }

        // fill grid
        public void fillGrid(List<Orders> orderList)
        {
            dataGridView2.Rows.Clear();
            using(var db = new LibData()) { 
            List<Authors> authorList = new List<Authors>();
                for (int i = 0; i < orderList.Count; i++)
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                    row.Cells[0].Value = orderList[i].id;
                    row.Cells[1].Value = orderList[i].Clients.Name + " " + orderList[i].Clients.Surname;

                    int bookid = orderList[i].BookID;
                    authorList = db.AuthorsBooks.Where(b => b.BookID == bookid).Select(ab => ab.Authors).ToList();
                    foreach (Authors author in authorList)
                    {
                        row.Cells[2].Value += author.Name + " " + author.Surname + " , ";
                    }

                    row.Cells[3].Value = orderList[i].OrderDate;
                    row.Cells[4].Value = orderList[i].ReturnDate;

                    if( DateTime.Compare(orderList[i].OrderDate, orderList[i].ReturnDate) > 0)
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;
                    }
                    dataGridView2.Rows.Add(row);
                }
            }
        }
    }
}
