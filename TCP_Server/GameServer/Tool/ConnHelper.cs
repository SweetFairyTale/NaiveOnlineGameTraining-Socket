/*
 * ConnHelper类与MyServer.Client交互
 * 用于建立以及销毁与数据库的连接
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GameServer.Tool
{
    class ConnHelper
    {
        //public const string CONNSTR = "datasource=127.0.0.1;port=3306;database=forestwar;user=root;pwd=0304";  //连接字符串.(Class 35)
        public const string CONNSTR = "datasource=127.0.0.1;port=3306;database=junglewar;user=root;pwd=0304";

        public static MySqlConnection Connect()
        {
            MySqlConnection conn = new MySqlConnection(CONNSTR);
            try
            {
                conn.Open();
                return conn;
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR]:Database Connectiong error:" + e);
                return null;
            }
        }

        public static void CloseConnection(MySqlConnection conn)
        {
            if(conn != null)
            {
                conn.Close();
            }
            else
            {
                Console.WriteLine("[ERROR]:MySqlConnection is null!(Close Fail)");
            }
        }

    }
}
