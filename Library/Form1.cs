using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    public partial class Form1 : Form
    {
        DBClasses.ClientDB clients = new DBClasses.ClientDB();
        DBClasses.BooksDB books = new DBClasses.BooksDB();
        DBClasses.AuthorDB authors = new DBClasses.AuthorDB();
        DBClasses.AuthorsBooksDB authorsBooksDB = new DBClasses.AuthorsBooksDB();
        DBClasses.CategoryDB categories = new DBClasses.CategoryDB();
        DBClasses.OrderDB orders = new DBClasses.OrderDB();
        int selectedBook = -1;
        int selectedClient = -1;
        public Form1()
        {
            InitializeComponent();
            refreshGrids();
        }
        // Search for clients
        private void button3_Click(object sender, EventArgs e)
        {
            SearchClient();
        }
        // on text chenged clients grid
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            SearchClient();
            if(textBox2.Text == string.Empty)
            {
                refreshGrids();
            }
        }
        // search for books
        private void button2_Click(object sender, EventArgs e)
        {
            SearchBook();
        }
        // on text chenged Books grid
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SearchBook();
            if (textBox1.Text == string.Empty)
            {
                refreshGrids();
            }
        }
        // Create order
        private void button1_Click(object sender, EventArgs e)
        {
            Orders order = new Orders();
            order.BookID = selectedBook;
            order.ClientID = selectedClient;
            order.OrderDate = DateTime.Now;
            order.ReturnDate = DateTime.Now.AddMonths(1);

            if (orders.Add(order))
            {
                txtInfo.Text = " Order is added";
                selectedBook = -1;
                selectedClient = -1;
                button1.Enabled = false;
            }
            else
            {
                txtInfo.Text = " Error: Order was not added";
            }
        }

        // Refresh Grids
        public void refreshGrids()
        {
            List<Clients> clientList = clients.SelectAll();
            dataGridView2.Rows.Clear();
            for (int i = 0; i < clientList.Count; i++)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                row.Cells[0].Value = clientList[i].id;
                row.Cells[1].Value = clientList[i].Name;
                row.Cells[2].Value = clientList[i].Surname;
                row.Cells[3].Value = clientList[i].Email;
                row.Cells[4].Value = clientList[i].Phone;
                dataGridView2.Rows.Add(row);
            }

            List<Books> bookList = books.SelectAll();
            dataGridView1.Rows.Clear();
            for (int i = 0; i < bookList.Count; i++)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                List<AuthorsBooks> authorBookList = authorsBooksDB.Select(bookList[i].id);

                row.Cells[0].Value = bookList[i].id;
                row.Cells[1].Value = bookList[i].Name;

                for (int j = 0; j < authorBookList.Count; j++)
                {
                    row.Cells[2].Value += authors.Select(authorBookList[j].AuthorID).Name + " " + authors.Select(authorBookList[j].AuthorID).Surname + ", ";
                }

                row.Cells[3].Value = bookList[i].Price;
                dataGridView1.Rows.Add(row);
            }
        }
        // Search for Client
        public void SearchClient()
        {
            string srcText = textBox2.Text;
            List<Clients> clientList = clients.GetClient(srcText);

            if (clientList.Count > 0)
            {
                dataGridView2.Rows.Clear();
                for (int i = 0; i < clientList.Count; i++)
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                    row.Cells[0].Value = clientList[i].id;
                    row.Cells[1].Value = clientList[i].Name;
                    row.Cells[2].Value = clientList[i].Surname;
                    row.Cells[3].Value = clientList[i].Email;
                    row.Cells[4].Value = clientList[i].Phone;
                    dataGridView2.Rows.Add(row);
                }

            }
        }
        // Search for book
        public void SearchBook()
        {
            string srcText = textBox2.Text;
            List<Books> bookList = new List<Books>();
            List<AuthorsBooks> abList = new List<AuthorsBooks>();
            bookList = books.GetBook(srcText);

            if(bookList.Count == 0)
            {
                List<Categories> categoryList = categories.GetCategory(srcText);
                foreach(Categories c in categoryList)
                {
                    bookList = books.GetBooksByCategory(c.id);
                }
            }

            if(bookList.Count == 0)
            {
                List<Authors> authorList = authors.GetAuthor(srcText);
                foreach (Authors c in authorList)
                {
                    abList.AddRange(authorsBooksDB.GetPairByAuthor(c.id));
                }

                foreach(AuthorsBooks ab in abList)
                {
                    bookList.Add(books.Select(ab.BookID));
                }

            }

            dataGridView1.Rows.Clear();
            for (int i = 0; i < bookList.Count; i++)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                List<AuthorsBooks> authorBookList = authorsBooksDB.Select(bookList[i].id);

                row.Cells[0].Value = bookList[i].id;
                row.Cells[1].Value = bookList[i].Name;

                for (int j = 0; j < authorBookList.Count; j++)
                {
                    row.Cells[2].Value += authors.Select(authorBookList[j].AuthorID).Name + " " + authors.Select(authorBookList[j].AuthorID).Surname + ", ";
                }

                row.Cells[3].Value = bookList[i].Price;
                dataGridView1.Rows.Add(row);
            }


        }

        private void authorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Subforms.AuthorForm author = new Subforms.AuthorForm();
            author.ShowDialog();
        }

        private void booksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Subforms.BookForm book = new Subforms.BookForm();
            book.ShowDialog();
        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Subforms.CathegoryForm category = new Subforms.CathegoryForm();
            category.ShowDialog();
        }

        private void clientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Subforms.ClientForm clientForm = new Subforms.ClientForm();
            clientForm.ShowDialog();
        }
        // select client
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedClient = (int) dataGridView2.Rows[e.RowIndex].Cells[0].Value;
            checkEnable();
        }
        // select book
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedBook = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            checkEnable();
        }

        public void checkEnable()
        {
            if(selectedBook >0 && selectedClient > 0)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }
    }
}
