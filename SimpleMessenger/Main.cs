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
    public partial class Main : Form
    {
        List<Global.MessageObject> MessageList;
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Login login = new Login();
            login.ShowDialog();
            if(Global.username == "")
            {
                this.Close();
            }

            setMessageTileSize();
            SQLQuery sqlq = new SQLQuery();
            sqlq.SetLogIn(Global.username);
            MessageList = new List<Global.MessageObject>();
            PopulateUserList();
        }

        private void setMessageTileSize()
        {
            double newWidth = lvMessages.Width - (lvMessages.Width * 0.05);
            double newHeight = 30;
            lvMessages.TileSize = new Size((int)newWidth,(int)newHeight);
        }

        private void PopulateUserList()
        {
            lbUser.Items.Clear();
            SQLQuery sqlq = new SQLQuery();
            List<string> Userlist = sqlq.GetLoggedInUserList();

            foreach(string item in Userlist)
            {
                lbUser.Items.Add(item);
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            SQLQuery sqlq = new SQLQuery();
            sqlq.SetLogOut(Global.username);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            PopulateUserList();
            //PopulateMessages();
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (txtboxMessage.Text.Trim() == "")
            {
                return;
            }

            SQLQuery sqlq = new SQLQuery();
            sqlq.EnterMessage(txtboxMessage.Text.Trim());
        }

        private void PopulateMessages()
        {
            string[] array = { "cat"};
            var items = lvMessages.Items;
            foreach (var value in array)
            {
                items.Add(value);
            }
        }

        private void Main_ClientSizeChanged(object sender, EventArgs e)
        {
            setMessageTileSize();
        }
    }
}
