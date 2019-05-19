using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Library.Subforms
{
    public partial class ClientForm : Form
    {
        Clients thisClient;
        DBClasses.ClientDB clients = new DBClasses.ClientDB();
        Validation validation = new Validation();
        public ClientForm()
        {
            InitializeComponent();
            RefreshGrid();
        }
        // Add or remove
        private void button3_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                if (thisClient == null)
                {
                    if (clients.Add(new Clients(txtName.Text, txtSurname.Text,txtPhone.Text, txtEmail.Text, true)))
                    {
                        txtInfo.Text = txtName.Text + " added";
                    }
                    else txtInfo.Text = "Error: Client was not added";
                }
                else
                {

                    if (clients.Update(new Clients(txtName.Text, txtSurname.Text, txtPhone.Text, txtEmail.Text, true), thisClient.id))
                    {
                        txtInfo.Text = "Client was updated";
                    }
                    else
                    {
                        txtInfo.Text = "Warning: Client was not updated";
                    }
                }
                ResetForm();
            }
        }
        //Delete
        private void button2_Click(object sender, EventArgs e)
        {
            if (clients.Delete(thisClient))
            {
                txtInfo.Text = "Client was deleted";
                ResetForm();
            }
            else txtInfo.Text = "Client is not deleted";
        }
        //Reset
        private void button1_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        // Cell double click
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = (int)dataGridView2.Rows[e.RowIndex].Cells[0].Value;
            thisClient = clients.Select(id);
            txtName.Text = thisClient.Name;
            txtSurname.Text = thisClient.Surname;
            txtEmail.Text = thisClient.Email;
            txtPhone.Text = thisClient.Phone;

            button3.Text = "Update";
            button2.Enabled = true;
        }

        // Reset all
        public void ResetForm()
        {
            thisClient = null;
            RefreshGrid();
            ClearInputs();
            button2.Enabled = false;
            button3.Text = "Save";
        }

        //Refresh grid
        public void RefreshGrid()
        {
            List<Clients> clientList = clients.SelectAll();
            dataGridView2.Rows.Clear();
            for (int i = 0; i < clientList.Count; i++)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                row.Cells[0].Value = clientList[i].id;
                row.Cells[1].Value = clientList[i].Name;
                row.Cells[2].Value = clientList[i].Surname;
                row.Cells[4].Value = clientList[i].Phone;
                row.Cells[3].Value = clientList[i].Email;
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
