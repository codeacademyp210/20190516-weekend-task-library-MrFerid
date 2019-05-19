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
    public partial class BookForm : Form
    {
        Validation validation = new Validation();
        DBClasses.BooksDB books = new DBClasses.BooksDB();
        DBClasses.AuthorsBooksDB authorsBooksDB = new DBClasses.AuthorsBooksDB();
        DBClasses.AuthorDB authors = new DBClasses.AuthorDB();
        DBClasses.CategoryDB categories = new DBClasses.CategoryDB();
        Books thisBook = null;
        public BookForm()
        {
            InitializeComponent();
            RefreshGrid();
            FillBoxes();
        }
        // Add or Update 
        private void button3_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                if (thisBook == null)
                {
                    Categories categ = cmbCathegory.SelectedItem as Categories;
                    int bookID = books.Add(new Books(txtName.Text, Convert.ToDouble(txtPrice.Text), categ.id, Convert.ToInt32(txtCount.Text)));

                    if (bookID > 0)
                    {
                        foreach (Authors item in listBox1.SelectedItems)
                        {
                            AuthorsBooks authorsBooks = new AuthorsBooks();
                            authorsBooks.BookID = bookID;
                            authorsBooks.AuthorID = item.id;
                            if (authorsBooksDB.Add(authorsBooks))
                            {
                                txtInfo.Text = "Book added";
                            }
                            else { txtInfo.Text = "Error: Author is not added"; }
                        }
                    }
                    else { txtInfo.Text = "Error: Book is not added"; }
                }
                else
                {   // Updating
                    Categories categ = cmbCathegory.SelectedItem as Categories;
                    if (books.Update(new Books(txtName.Text, Convert.ToDouble(txtPrice.Text), categ.id, Convert.ToInt32(txtCount.Text)), thisBook.id))
                    {
                        txtInfo.Text = "Book is Updated!";
                    }
                    else
                    {
                        txtInfo.Text = "Warning: book is not updated!";
                    }
                    // Update all authors
                    authorsBooksDB.DeleteRows(thisBook.id); // first remove all authors
                    foreach (Authors item in listBox1.SelectedItems)
                    {
                        AuthorsBooks authorsBooks = new AuthorsBooks();
                        authorsBooks.BookID = thisBook.id;
                        authorsBooks.AuthorID = item.id;

                       if( authorsBooksDB.Add(authorsBooks))
                        {
                            txtInfo.Text = "Book is Updated!";
                        }
                    }
                }

                ResetForm();
                button3.Text = "Save";
            }
        }

        // Double click on cell
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int bookID = (int) dataGridView2.Rows[e.RowIndex].Cells[0].Value;
            thisBook =  books.Select(bookID);
            button2.Enabled = true;
            button3.Text = "Update";

            Categories currCateg = categories.Select(thisBook.CategoryID); // getting category of this book
            List<AuthorsBooks> abList = authorsBooksDB.Select(thisBook.id); // getting author id.s for this book
            
            txtName.Text = thisBook.Name;
            txtPrice.Text = thisBook.Price.ToString();
            txtCount.Text = thisBook.Count.ToString();

            foreach(Categories c in cmbCathegory.Items)
            {
                if(c.id == currCateg.id)
                {
                    cmbCathegory.SelectedItem = c;
                }
            }

            listBox1.SelectedIndex = -1;
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                Authors item = listBox1.Items[i] as Authors;
                for(int j=0; j< abList.Count; j++)
                {
                    Authors author = authors.Select(abList[j].AuthorID);
                    if(item.id == author.id)
                    {
                        listBox1.SelectedIndex = i;
                    }
                }
            }
        }

        // Delete button
        private void button2_Click(object sender, EventArgs e)
        {
            if (authorsBooksDB.DeleteRows(thisBook.id) && books.Delete(thisBook))
            {
                txtInfo.Text = "Book is deleted !";
                RefreshGrid();
                ResetForm();
            }
            else
            {
                txtInfo.Text = "Error: Book was not deleted!";
            }
        }

        // Reset button
        private void button1_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        // Reset all
        public void ResetForm()
        {
            thisBook = null;
            RefreshGrid();
            ClearInputs();
            listBox1.SelectedIndex = -1;
            cmbCathegory.SelectedIndex = -1;
            button2.Enabled = false;
            button3.Text = "Save";
        }

        //Refresh grid
        public void RefreshGrid()
        {
            List<Books> bookList = books.SelectAll();
            dataGridView2.Rows.Clear();
            for (int i = 0; i < bookList.Count; i++)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                List<AuthorsBooks> authorBookList = authorsBooksDB.Select(bookList[i].id);

                row.Cells[0].Value = bookList[i].id;
                row.Cells[1].Value = bookList[i].Name;

                for(int j=0; j < authorBookList.Count; j++)
                {
                    row.Cells[2].Value += authors.Select(authorBookList[j].AuthorID).Name + " " + authors.Select(authorBookList[j].AuthorID).Surname + ", ";
                }
            
                row.Cells[3].Value = bookList[i].Price;
                row.Cells[4].Value = categories.Select(bookList[i].CategoryID).Name;
                row.Cells[5].Value = bookList[i].Count;

                dataGridView2.Rows.Add(row);
            }           
        }
        // Fill listBox and Combobox
        public void FillBoxes()
        {
            List<Authors> authorList = authors.SelectAll();
            List<Categories> categoryList = categories.SelectAll();
            List<string> Fullname = authorList.Select(s => s.Name).ToList();
            listBox1.Items.Clear();

            listBox1.DataSource = authorList;
            listBox1.DisplayMember = "Name";

            cmbCathegory.DataSource = categoryList;
            cmbCathegory.DisplayMember = "Name";
        }

        //Clear all inputs
        public void ClearInputs()
        {
            foreach (Control con in panel1.Controls)
            {
                if (con is TextBox)
                {
                    con.Text = string.Empty;
                }
            }
        }

        // Form validation
        public bool isValid()
        {
            int valid = 0;
            int inputCount = 0;
            foreach (Control con in panel1.Controls)
            {
                if (con is TextBox)
                {
                    inputCount++;
                    if (validation.CheckInput(con.AccessibleName, con.Text, con.AccessibleDescription) == "ok")
                    {
                        valid++;
                    }
                    else
                    {
                        ToolTip toolTip = new ToolTip();
                        toolTip.IsBalloon = true;
                        toolTip.Show(validation.CheckInput(con.AccessibleName, con.Text, con.AccessibleDescription), con, 120, -40, 2000);
                    }
                }
            }
            if (valid == inputCount) { return true; }
            else { return false; }
        }

    }
}
