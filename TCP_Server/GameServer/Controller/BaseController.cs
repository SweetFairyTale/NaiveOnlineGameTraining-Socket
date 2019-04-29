﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;  //自定义类库.

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

        //ActionCode未指定时默认调用的消息处理方法.
        public virtual string DefaultHandler(string data)
        {
            return null;
        }
    }
}
