﻿using System;
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
            SQLQuery sqlq = new SQLQuery();
            sqlq.SetLogOut(Global.username);
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
                item.Text = "[" + value.user+"] : "+value.Message;
                lvMessages.Items.Add(item);
            }
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(lvMessages.Items.Count == 0)
            {
                PopulateRecentMessages();
            }
            else
            {
                PopulateRecentMessages((int)(lvMessages.Items[0].Tag));
            }
        }
    }
}
