using System.Data.SqlClient;
using System.Data;
using System;
using System.IO;
using System.Collections.Generic;

namespace SimpleMessenger {
    public class SQLQuery
    {
        public void CreateLogin(string user, string pass)
        {
            SqlConnection conn = new SqlConnection(Global.connectionStr);
            SqlCommand cmd = new SqlCommand("AddUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@user", SqlDbType.NVarChar);
            cmd.Parameters.Add("@password", SqlDbType.NVarChar);
            cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

            cmd.Parameters["@user"].Value = user;
            cmd.Parameters["@password"].Value = pass;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();

                if (cmd.Parameters["@output"].Value.ToString() == "1")
                {
                    conn.Close();
                    throw new Exception("Username already exists");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception(ex.Message);
            }
        }
        public void ValidateLogin(string user, string pass)
        {
            SqlConnection conn = new SqlConnection(Global.connectionStr);
            SqlCommand cmd = new SqlCommand("CheckLogin", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@user", SqlDbType.NVarChar);
            cmd.Parameters.Add("@password", SqlDbType.NVarChar);
            cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

            cmd.Parameters["@user"].Value = user;
            cmd.Parameters["@password"].Value = pass;
            try {
                conn.Open();
                cmd.ExecuteNonQuery();

                if (cmd.Parameters["@output"].Value.ToString() == "1")
                {
                    conn.Close();
                    throw new Exception("Incorrect Username/Password");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception(ex.Message);
            }
        }

        public void SetLogIn(string user)
        {
            SqlConnection conn = new SqlConnection(Global.connectionStr);
            SqlCommand cmd = new SqlCommand("Login", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@user", SqlDbType.NVarChar);

            cmd.Parameters["@user"].Value = user;
            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception(ex.Message);
            }
        }

        public void SetLogOut(string user)
        {
            SqlConnection conn = new SqlConnection(Global.connectionStr);
            SqlCommand cmd = new SqlCommand("Logout", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@user", SqlDbType.NVarChar);

            cmd.Parameters["@user"].Value = user;
            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception(ex.Message);
            }
        }

        public List<string> GetLoggedInUserList()
        {
            List<string> result = new List<string>();
            SqlConnection conn = new SqlConnection(Global.connectionStr);
            SqlCommand cmd = new SqlCommand("GetLoggedInUserList", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter sqladap = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sqladap.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                result.Add(row["User"].ToString());
            }

            return result;
        }

        public List<Global.MessageObject> GetRecentMessages(int messageid = -1)
        {
            List<Global.MessageObject> result = new List<Global.MessageObject>();

            SqlConnection conn = new SqlConnection(Global.connectionStr);
            SqlCommand cmd = new SqlCommand("GetRecentMessages", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (messageid != -1)
            {
                cmd.Parameters.Add("@messageid", SqlDbType.NVarChar);
                cmd.Parameters["@messageid"].Value = messageid;
            }

            SqlDataAdapter sqladap = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sqladap.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                Global.MessageObject MsgObj = new Global.MessageObject();

                MsgObj.user = row["User"].ToString();
                MsgObj.Message = row["Message1"].ToString();
                MsgObj.MessageID = (int)row["MessageID"];
                MsgObj.datetime = DateTime.Parse(row["timestamp"].ToString());

                result.Add(MsgObj);
            }

            return result;
        }

        public void EnterMessage(string message)
        {
            SqlConnection conn = new SqlConnection(Global.connectionStr);
            SqlCommand cmd = new SqlCommand("EnterMessage", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@user", SqlDbType.NVarChar);
            cmd.Parameters.Add("@message", SqlDbType.NVarChar);
            cmd.Parameters["@user"].Value = Global.username;
            cmd.Parameters["@message"].Value = message;

            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception(ex.Message);
            }
        }

        public List<Global.MessageObject> GetPastMessages(int messageid)
        {
            List<Global.MessageObject> result = new List<Global.MessageObject>();

            SqlConnection conn = new SqlConnection(Global.connectionStr);
            SqlCommand cmd = new SqlCommand("GetPastMessages", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@messageid", SqlDbType.NVarChar);
            cmd.Parameters["@messageid"].Value = messageid;

            SqlDataAdapter sqladap = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sqladap.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                Global.MessageObject MsgObj = new Global.MessageObject();

                MsgObj.user = row["User"].ToString();
                MsgObj.Message = row["Message1"].ToString();
                MsgObj.MessageID = (int)row["MessageID"];
                MsgObj.datetime = DateTime.Parse(row["timestamp"].ToString());

                result.Add(MsgObj);
            }

            return result;
        }
    }
}