using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SQL_Manager
{
    class Program
    {
        static void Main(string[] args)
        {
            string connStr = "Database=test01;datasource=127.0.0.1;port=3306;user=sa;pwd=123";
            MySqlConnection conn = new MySqlConnection(connStr);

            conn.Open();

            #region 查询
            //MySqlCommand cmd1 = new MySqlCommand("select * from user", conn);
            //MySqlDataReader reader = cmd.ExecuteReader();

            //if(reader.Read())
            //{
            //    string username = reader.GetString("username");  //根据列名获取.
            //    string password = reader.GetString("password");
            //    Console.WriteLine(username + ':' + password);
            //}
            //reader.Close();
            #endregion

            string username = "baba";string password = "gav";
            //MySqlCommand cmd = new MySqlCommand("insert into user set username = '" + username + "'" + "',password = '" + password + "'", conn);
            MySqlCommand cmd = new MySqlCommand("insert into user set username = @utl, password = @pwd", conn);

            cmd.Parameters.AddWithValue("utl", username);
            cmd.Parameters.AddWithValue("pwd", password);
            cmd.ExecuteNonQuery();

            conn.Close();

            Console.ReadKey();
        }
    }
}
