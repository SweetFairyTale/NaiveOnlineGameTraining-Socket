using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Reflection; //反射机制(?)
using GameServer.MyServer;

namespace GameServer.Controller
{
    /// <summary>
    /// 管理所有的控制器Controller.
    /// </summary>
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();

        private Server myServer;

        public ControllerManager(Server server)
        {
            myServer = server;
            InitController();
        }

        //初始化controller.
        private void InitController()
        {
            DefaultController defaultController = new DefaultController();
            controllerDict.Add(defaultController.RequestCode, defaultController);
        }

        //RequestCode未指定时默认调用的消息处理方法.
        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            BaseController controller;
            bool isGet = controllerDict.TryGetValue(requestCode, out controller);
            if(isGet == false)
            {
                Console.WriteLine("[" + requestCode + "]no matching function");  //或输出到日志.
                return;
            }

            string methodName = Enum.GetName(typeof(ActionCode), actionCode);  //转换值枚举为字符串.
            MethodInfo mi = controller.GetType().GetMethod(methodName);  //根据函数名得到mi.
            if(mi == null)
            {
                Console.WriteLine("[" + controller.GetType() + "]no matching function ->" + methodName);
                return;
            }
            object[] parameters = new object[] { data };
            object obj = mi.Invoke(controller, parameters);  //通过mi在controller对象中调用methodName的方法，parameters可用于传递一组参数，调用结果返回值为obj.
            if(obj == null || string.IsNullOrEmpty(obj as string))
            {
                return;
            }
            myServer.SendResponse(client, requestCode, obj as string);
        }
    }
}
