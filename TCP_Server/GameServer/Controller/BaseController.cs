using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;  //自定义类库.
using GameServer.MyServer;

namespace GameServer.Controller
{
    abstract class BaseController  //controller基础类.
    {
        RequestCode requestCode = RequestCode.None;

        public RequestCode RequestCode
        {
            get
            {
                return requestCode;
            }
        }

        //
        /// <summary>
        /// BaseController提供ActionCode未指定时默认调用的消息处理方法.
        /// 后续其他继承该类的Controller重写该方法实现不同请求处理.
        /// </summary>
        /// <param name="data">由Server解析得到的真实数据</param>
        /// <param name="client">当一个请求涉及其他客户端时，提供客户端引用</param>
        /// <param name="server">同client</param>
        /// <returns>需要发回客户端的数据，默认为null表示不需要反馈</returns>
        public virtual string DefaultHandler(string data, Client client, Server server)
        {
            return null;
        }
    }
}
