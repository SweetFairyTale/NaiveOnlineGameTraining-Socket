using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Model;
using MySql.Data.MySqlClient;
using Common;

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
                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    int totalCount = reader.GetInt32("totalcount");
                    int winCount = reader.GetInt32("wincount");

                    return new History(id, userid, totalCount, winCount);
                }
                else
                {
                    return new History(Constant.NO_HISTORY_FOUND, userid, 0, 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR]:Fail To Get User History While \'GetHistoryByUserid\': " + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }

        public void NewOrUpdateHistory(MySqlConnection conn, History history)
        {
            try
            {
                MySqlCommand cmd = null;
                if (history.Id == Constant.NO_HISTORY_FOUND)
                {
                    cmd = new MySqlCommand("insert into history set userid = @userid,totalcount = @totalcount,wincount = @wincount", conn);
                }
                else
                {
                    cmd = new MySqlCommand("update history set totalcount = @totalcount,wincount = @wincount where userid = @userid", conn);
                }
                cmd.Parameters.AddWithValue("totalcount", history.TotalCount);
                cmd.Parameters.AddWithValue("wincount", history.WinCount);
                cmd.Parameters.AddWithValue("userid", history.UserId);
                cmd.ExecuteNonQuery();

                if(history.Id == Constant.NO_HISTORY_FOUND)
                {
                    /* 第一次创建用户History时，服务器程序中的抽象history对象Id被设为了-1
                     * 这个对象会一直被保留直到该用户重新登录，即用户在第一次登录并进行多次游戏的情形下
                     * 这个Id随后会错误的保持-1造成每次更新战绩都重复插入语句(但数据库中Id使用自增方式不为-1)
                     * 因此通过判断将第一次插入的数据更新到服务器history对象
                     */
                    History tempHistory = GetHistoryByUserid(conn, history.UserId);
                    history.Id = tempHistory.Id;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR]:Fail To New/Update User History: " + e);
            }
        }
    }
}
