using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using GameServer.Model;

namespace GameServer.DAO
{
    class UserDAO
    {
        /// <summary>
        /// 在数据库中验证用户名和密码，成功时返回一个User对象，否则返回null.
        /// </summary>
        public User AuthenticatingUser(MySqlConnection conn, string username, string password)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username = @username and password = @password"
                                                 , conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    User user = new User(reader.GetInt32("id"), username, password);
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR]:Unauthenticated User:" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }

        /// <summary>
        /// 在数据库中验证查找是否已存在某个名称的用户，存在时返回true，否则返回false.
        /// </summary>
        public bool GetUserByUsername(MySqlConnection conn, string username)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username = @username", conn);
                cmd.Parameters.AddWithValue("username", username);
                reader = cmd.ExecuteReader();
                return reader.HasRows;
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR]:Fail To Get User By Username:" + e);
            }
            finally
            {
                if (reader != null) reader.Close();                
            }
            return false;
        }

        /// <summary>
        /// 在数据库中验证查找是否已存在某个名称的用户
        /// </summary>
        public void RegisterUser(MySqlConnection conn, string username, string password)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into user set username = @username, password = @password", conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                cmd.ExecuteNonQuery();  //返回受影响行数.
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR]:Fail To Register User:" + e);
            }
        }
    }
}
