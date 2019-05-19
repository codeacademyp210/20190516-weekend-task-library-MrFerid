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
    public partial class AuthorForm : Form
    {
        DBClasses.AuthorDB authors = new DBClasses.AuthorDB();
        Validation validation = new Validation();
        Authors thisAuthor = null;
   
        public AuthorForm()
        {
            InitializeComponent();
            RefreshGrid();
        }
        // Add or update Author
        private void button3_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                if (thisAuthor == null)
                {
                    if (authors.Add(new Authors(textBox1.Text, textBox2.Text)))
                    {
                       txtInfo.Text = textBox1.Text + " added";
                    }
                    else txtInfo.Text = "Error: Author was not added";
                }
                else
                {

                    if (authors.Update(new Authors(textBox1.Text,textBox2.Text),thisAuthor.id))
                    {
                        txtInfo.Text = "Author was updated";
                    }
                    else
                    {
                        txtInfo.Text = "Warning: Author was not updated";
                    }
                }

                ResetForm();
                button3.Text = "Save";
            }

        }
        //Double click on cell
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = (int)dataGridView2.Rows[e.RowIndex].Cells[0].Value;
            thisAuthor = authors.Select(id);
            textBox1.Text = thisAuthor.Name;
            textBox2.Text = thisAuthor.Surname;
            button3.Text = "Update";
        }

        // Delete Author
        private void button2_Click(object sender, EventArgs e)
        {
            if (authors.Delete(thisAuthor))
            {
                txtInfo.Text = "Author was deleted";
                ResetForm();
            }
            else txtInfo.Text = "Author is not deleted";
        }

        //Resetting
        private void button1_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        // Reset all
        public void ResetForm()
        {
            thisAuthor = null;
            RefreshGrid();
            ClearInputs();
        }

        //Refresh grid
        public void RefreshGrid()
        {
            List<Authors> authorList = authors.SelectAll();
            dataGridView2.Rows.Clear();
            for (int i = 0; i < authorList.Count; i++)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                row.Cells[0].Value = authorList[i].id;
                row.Cells[1].Value = authorList[i].Name;
                row.Cells[2].Value = authorList[i].Surname;
                dataGridView2.Rows.Add(row);

            }
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
