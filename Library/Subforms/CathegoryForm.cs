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
    public partial class CathegoryForm : Form
    {
        DBClasses.CategoryDB categories = new DBClasses.CategoryDB();
        Categories thisCategory = null;
        public CathegoryForm()
        {
            InitializeComponent();
        }
        // Add or Update Category
        private void button3_Click(object sender, EventArgs e)
        {
            if(txtName.Text != string.Empty)
            {
                if (thisCategory == null)
                {
                    if (categories.Add(new Categories(txtName.Text)))
                    {
                        txtInfo.Text = "Category added";
                    }
                    else { txtInfo.Text = "Error: Category is not added"; }
                }
                else
                {
                    if (categories.Update(new Categories(txtName.Text), thisCategory.id))
                    {
                        txtInfo.Text = "Category was Updated";
                    }
                    else txtInfo.Text = "Warning: Category is not updated";

                }

                Reset();
            }
            else
            {
                txtInfo.Text = "Type Category name";
            }
        }
        // Cell double click
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = (int)dataGridView2.Rows[e.RowIndex].Cells[0].Value;
            thisCategory = categories.Select(id);
            txtName.Text = thisCategory.Name;
            button3.Text = "Update";
        }
        // Delete category
        private void button2_Click(object sender, EventArgs e)
        {
            if (categories.Delete(thisCategory))
            {
                txtInfo.Text = "Category Removed";
                Reset();
            }
            else { txtInfo.Text = "Error: Category not removed"; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Reset();
        }
        // Reset Method
        public void Reset()
        {
            thisCategory = null;
            ClearInputs();
            RefreshGrid();
            button3.Text = "Save";
        }

        //Refresh grid
        public void RefreshGrid()
        {
            List<Categories> categoryList = categories.SelectAll();
            dataGridView2.Rows.Clear();
            for (int i = 0; i < categoryList.Count; i++)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                row.Cells[0].Value = categoryList[i].id;
                row.Cells[1].Value = categoryList[i].Name;
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

    }
}
