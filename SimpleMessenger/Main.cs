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
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SimpleMessenger
{
    public partial class Main : Form
    {
        int lastreadMessage = -1;
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

            lvMessages.Items[lvMessages.Items.Count - 1].EnsureVisible();
            lastreadMessage = lvMessages.Items.Count - 1;
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
                if (ApplicationIsActivated() && lastreadMessage != -1)
                {
                    if (lvMessages.Items[lvMessages.Items.Count - 1].Bounds.IntersectsWith(lvMessages.ClientRectangle))
                    {
                        lvMessages.Items[lastreadMessage].EnsureVisible();
                        lastreadMessage = lvMessages.Items.Count - 1;
                    }
                    if (lvMessages.Items[0].Bounds.IntersectsWith(lvMessages.ClientRectangle))
                    {
                        SQLQuery sqlq = new SQLQuery();
                        Global.MessageList.Clear();
                        Global.MessageList.AddRange(sqlq.GetPastMessages((int)lvMessages.Items[0].Tag));

                        foreach (Global.MessageObject value in Global.MessageList)
                        {
                            ListViewItem item = new ListViewItem();
                            item.Tag = value.MessageID;
                            item.Text = "[" + value.datetime.ToString() + "]";
                            item.Name = "Time";
                            ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem(item, "Message");
                            subItem.Name = "Message";
                            subItem.Text = "[" + value.user + "] : " + value.Message;
                            item.SubItems.Add(subItem);
                            lvMessages.Items.Insert(0, item);
                            lastreadMessage++;
                            
                        }
                    }
                }
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
            lvMessages.Items[lvMessages.Items.Count - 1].EnsureVisible();
            lastreadMessage = lvMessages.Items.Count - 1;
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

        /// <summary>Returns true if the current application has focus, false otherwise</summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
        public static bool ApplicationIsActivated()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }
    }
}
