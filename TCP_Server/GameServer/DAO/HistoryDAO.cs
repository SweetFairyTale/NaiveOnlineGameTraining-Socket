using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Model;
using MySql.Data.MySqlClient;

namespace GameServer.DAO
{
    class HistoryDAO
    {
        public History GetHistoryByUserid(MySqlConnection conn, int userid)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from history where userid = @userid", conn);
                cmd.Parameters.AddWithValue("userid", userid);
                reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    int id = reader.GetInt32("id");
                    int totalCount = reader.GetInt32("totalcount");
                    int winCount = reader.GetInt32("wincount");
                   
                    return new History(id, userid, totalCount, winCount);
                }
                else
                {
                    return new History(-1, userid, 0, 0);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("[ERROR]:Fail To Get User History" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }
    }
}
