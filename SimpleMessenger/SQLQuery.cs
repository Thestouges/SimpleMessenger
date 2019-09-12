using System.Data.SqlClient;
using System.Data;
using System;
using System.IO;

public class SQLQuery
{
    string connectionStr;

    public SQLQuery()
    {
        connectionStr = File.ReadAllText("SQLConnectionString.txt");
    }

    public void ValidateLogin(string user, string pass)
    {
        SqlConnection conn = new SqlConnection(connectionStr);
        SqlCommand cmd = new SqlCommand("CheckLogin",conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@user", SqlDbType.NVarChar);
        cmd.Parameters.Add("@password", SqlDbType.NVarChar);
        cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

        cmd.Parameters["@user"].Value = user;
        cmd.Parameters["@password"].Value = user;
        try {
            conn.Open();
            cmd.ExecuteNonQuery();

            if(cmd.Parameters["@output"].Value.ToString() == "1")
            {
                conn.Close();
                throw new Exception("Incorrect Username/Password");
            }

            conn.Close();
        }
        catch(Exception ex)
        {
            conn.Close();
            throw new Exception(ex.Message);
        }
    }

}
