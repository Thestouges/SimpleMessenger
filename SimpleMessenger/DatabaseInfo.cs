using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SimpleMessenger
{
    public partial class DatabaseInfo : Form
    {
        public DatabaseInfo()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if(txtboxCatalog.Text.Trim() == "" 
                || txtboxDatasource.Text.Trim() == ""
                || txtboxPassword.Text.Trim() == ""
                || txtboxUserid.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter all values in all fields");
                return;
            }

            try
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString =
                    "Data Source="+txtboxDatasource.Text+";" +
                    "Initial Catalog="+txtboxCatalog.Text+";" +
                    "User id="+txtboxUserid.Text+";" +
                    "Password="+txtboxPassword.Text+";";
                conn.Open();
                conn.Close();
                Global.connectionStr =
                    "Data Source=" + txtboxDatasource.Text + ";" +
                    "Initial Catalog=" + txtboxCatalog.Text + ";" +
                    "User id=" + txtboxUserid.Text + ";" +
                    "Password=" + txtboxPassword.Text + ";";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
