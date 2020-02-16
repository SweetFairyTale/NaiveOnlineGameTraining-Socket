/*
 * ConnHelper类与MyServer.Client交互
 * 用于建立与数据库的连接
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
        public const string CONNSTR = "datasource=*:*:*:*;port=3306;database=;user=;pwd=";  //连接字符串.(Class 35)

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
                Console.WriteLine("[ERROR]:MySqlConnection is null!");
            }
        }

    }
}
