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
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string srctxt = txtName.Text;
             //Vaxt catmadi :)

        }
    }
}
