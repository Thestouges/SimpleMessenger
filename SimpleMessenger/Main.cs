using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SimpleMessenger
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (!File.Exists("SQLConnectionString.txt"))
            {
                DatabaseInfo getconnStr = new DatabaseInfo();
                getconnStr.ShowDialog();
            }
            else
            {
                Global.connectionStr = File.ReadAllText("SQLConnectionString.txt");
            }

            if(Global.connectionStr == "")
            {
                this.Close();
            }

            Login login = new Login();
            login.ShowDialog();
            if(Global.username == "")
            {
                this.Close();
            }

            timer1.Enabled = true;
            SQLQuery sqlq = new SQLQuery();
            sqlq.SetLogIn(Global.username);
            Global.MessageList = new List<Global.MessageObject>();
            PopulateUserList();
            PopulateRecentMessages();

            
            this.AcceptButton = btnSend;

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
            try
            {
                SQLQuery sqlq = new SQLQuery();
                sqlq.SetLogOut(Global.username);
            }
            catch
            {

            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            PopulateUserList();
            try
            {
                PopulateRecentMessages((int)(lvMessages.Items[lvMessages.Items.Count - 1].Tag));
            }
            catch(Exception ex)
            {

            }
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (txtboxMessage.Text.Trim() == "")
            {
                return;
            }

            SQLQuery sqlq = new SQLQuery();
            sqlq.EnterMessage(txtboxMessage.Text.Trim());
            txtboxMessage.Text = "";
            PopulateRecentMessages((int)(lvMessages.Items[lvMessages.Items.Count - 1].Tag));
        }

        private void PopulateRecentMessages(int index = -1)
        {
            SQLQuery sqlq = new SQLQuery();
            Global.MessageList.Clear();
            if (index == -1)
            {
                Global.MessageList.AddRange(sqlq.GetRecentMessages());
            }
            else
            {
                Global.MessageList.AddRange(sqlq.GetRecentMessages(index));
            }

            foreach (Global.MessageObject value in Global.MessageList)
            {
                ListViewItem item = new ListViewItem();
                item.Tag = value.MessageID;
                item.Text = "[" + value.datetime.ToString()+"]";
                item.Name = "Time";
                ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem(item, "Message");
                subItem.Name = "Message";
                subItem.Text = "[" + value.user + "] : " + value.Message;
                item.SubItems.Add(subItem);
                lvMessages.Items.Add(item);
            }

            lvMessages.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lvMessages.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(lvMessages.Items.Count == 0)
            {
                PopulateRecentMessages();
            }
            else
            {
                PopulateRecentMessages((int)(lvMessages.Items[lvMessages.Items.Count - 1].Tag));
            }
        }
    }
}
