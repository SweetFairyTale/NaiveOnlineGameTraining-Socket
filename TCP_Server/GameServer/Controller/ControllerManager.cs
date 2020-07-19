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

        private Server myServer;  //持有Server引用.

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
            controllerDict.Add(RequestCode.User, new UserController());
            controllerDict.Add(RequestCode.Room, new RoomController());
            controllerDict.Add(RequestCode.Game, new GameController());
        }

        /// <summary>
        /// 根据Code提供多种对请求的处理方法，使用反射机制.
        /// RequestCode未指定时默认调用的消息处理方法.
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="actionCode"></param>
        /// <param name="data"></param>
        /// <param name="client"></param>
        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            BaseController controller;
            bool isGet = controllerDict.TryGetValue(requestCode, out controller);
            if(isGet == false)
            {
                Console.WriteLine("[ERROR]:" + "[" + requestCode + "] no matching controller-class");
                return;
            }
        
            string methodName = Enum.GetName(typeof(ActionCode), actionCode);  //转换值枚举为字符串，从而根据actionCode得到方法名.
            MethodInfo mi = controller.GetType().GetMethod(methodName);  //根据方法名得到mi.
            if(mi == null)
            {
                Console.WriteLine("[ERROR]:" + "[" + controller.GetType() + "] no matching function ->" + methodName);
                return;
            }

            object[] parameters = new object[] { data, client, myServer };
            object obj = mi.Invoke(controller, parameters);  //通过mi在controller对象中调用methodName的方法，parameters可用于传递一组参数，调用结果返回值存放在obj.
            if(obj == null || string.IsNullOrEmpty(obj as string))  //不需要给客户端反馈.
            {
                return;
            }
            myServer.SendResponse(client, actionCode, obj as string);  //需要给客户端反馈.
        }
    }
}
