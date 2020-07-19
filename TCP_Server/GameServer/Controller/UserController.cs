using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.MyServer;
using GameServer.DAO;
using GameServer.Model;
using Common;

namespace GameServer.Controller
{
    class UserController : BaseController
    {
        private UserDAO userDAO = new UserDAO();
        private HistoryDAO historyDAO = new HistoryDAO();
        public UserController()
        {
            requestCode = RequestCode.User;
        }

        /// <summary>
        /// 对应ActionCode中的Login的同名方法，处理用户登录请求.
        /// </summary>
        public string Login(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            User user = userDAO.AuthenticatingUser(client.MysqlConn, strs[0], strs[1]);
            if(user == null)
            {
                Console.WriteLine("[WARNING]:No User Found or Wrong Password");
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                Console.WriteLine("[SUCCESS]:" + user.Username + " Login Success");

                History history = historyDAO.GetHistoryByUserid(client.MysqlConn, user.Id);
                client.SetUserData(user, history);

                Console.WriteLine("Read Player History: TotalCount:[" + history.TotalCount + "], WinCount:[" + history.WinCount + "]");
                return string.Format("{0},{1},{2},{3}", ((int)ReturnCode.Success).ToString(), user.Username, history.TotalCount, history.WinCount);
            }
        }

        /// <summary>
        /// 对应ActionCode中的Register的同名方法，处理用户注册请求.
        /// </summary>
        public string Register(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');

            bool res = userDAO.GetUserByUsername(client.MysqlConn, strs[0]);
            if(res)
            {
                //用户名重复，注册失败.
                return ((int)ReturnCode.Fail).ToString();
            }
            
            userDAO.RegisterUser(client.MysqlConn, strs[0], strs[1]);
            Console.WriteLine("[SUCCESS]:Register User:" + strs[0]);
            return ((int)ReturnCode.Success).ToString();
        }

    }
}
