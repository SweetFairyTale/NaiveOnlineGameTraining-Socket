using System;
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

        public virtual void DefaultHandler() { }
    }
}
