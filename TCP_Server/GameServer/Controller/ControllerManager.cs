using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Reflection; //反射机制(?)

namespace GameServer.Controller
{
    /// <summary>
    /// 管理所有的控制器Controller.
    /// </summary>
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();

        public ControllerManager()
        {
            Init();
        }

        private void Init()
        {
            DefaultController defaultController = new DefaultController();
            controllerDict.Add(defaultController.RequestCode, defaultController);
        }

        //RequestCode未指定时默认调用的消息处理方法.
        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data)
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
            object obj = mi.Invoke(controller, parameters);
        }
    }
}
