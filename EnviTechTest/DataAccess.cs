using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnviTechTest
{
    class DataAccess
    {
        static private string connString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=E:\test.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        static public DataTable GetDataTable(string strSql, string table_name)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connString);
            SqlCommand command = new SqlCommand(strSql, connection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(strSql, connection);
            dataAdapter.Fill(dt);
            dt.TableName = table_name;
            return dt;
        }

        public static Object ExecuteScalar(string strSql)
        {
            SqlConnection connection = new SqlConnection(connString);
            SqlCommand command = new SqlCommand(strSql, connection);
            connection.Open();
            Object obj = command.ExecuteScalar();
            connection.Close();
            return obj;
        }

        static public DataTable GetAllDataTable()
        {
            return GetDataTable("SELECT * FROM DATA", "DATA");
        }
    }
}
