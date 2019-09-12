using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleMessenger
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            SQLQuery sqlFunction = new SQLQuery();
            try
            {
                sqlFunction.ValidateLogin(txtboxUser.Text,txtboxPass.Text);
                Global.username = txtboxUser.Text;
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void BtnCreateNew_Click(object sender, EventArgs e)
        {

        }
    }
}
