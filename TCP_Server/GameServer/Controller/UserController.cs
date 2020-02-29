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
                return ((int)ReturnCode.Fail).ToString();  //1
            }
            else
            {
                Console.WriteLine("[SUCCESS]:" + user.Username + " Login Success");
                return ((int)ReturnCode.Success).ToString();  //0
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
            return ((int)ReturnCode.Success).ToString();
        }

    }
}
