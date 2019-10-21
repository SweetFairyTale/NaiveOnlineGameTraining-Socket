using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

//测试MySQL连接和增删改查.
//首先在项目"引用-浏览"中添加数据库支持dll文件.
namespace SQL_Manager
{
    class Program
    {
        static void Main(string[] args)
        {
            //!!
            string connStr = "Database=netserver01;datasource=127.0.0.1;port=3306;user=root;pwd=Xlm8504824";
            //!!

            MySqlConnection conn = new MySqlConnection(connStr);

            conn.Open();

            #region 查询
            //MySqlCommand cmd1 = new MySqlCommand("select * from user", conn);
            //MySqlDataReader reader = cmd1.ExecuteReader();

            //if (reader.Read())
            //{
            //    string username = reader.GetString("username");  //根据列名获取.
            //    string password = reader.GetString("password");
            //    Console.WriteLine(username + ':' + password);
            //}
            //reader.Close();
            #endregion

            #region 插入
            string username = "baba"; string password = "gav";
            //MySqlCommand cmd = new MySqlCommand("insert into user set username = '" + username + "'" + "',password = '" + password + "'", conn);
            MySqlCommand cmd2 = new MySqlCommand("insert into user set username = @utl, password = @pwd", conn);

            cmd2.Parameters.AddWithValue("utl", username);
            cmd2.Parameters.AddWithValue("pwd", password);
            cmd2.ExecuteNonQuery();
            #endregion

            #region 删除
            MySqlCommand cmd3 = new MySqlCommand("delete form user where id = @id", conn);
            cmd3.Parameters.AddWithValue("id", 3);
            cmd3.ExecuteNonQuery();
            #endregion

            #region 更新
            MySqlCommand cmd4 = new MySqlCommand("update user set password = @pwd where id = 3", conn);
            cmd4.Parameters.AddWithValue("pwd", "didi");
            cmd4.ExecuteNonQuery();
            #endregion

            conn.Close();
            Console.ReadKey();
        }
    }
}
