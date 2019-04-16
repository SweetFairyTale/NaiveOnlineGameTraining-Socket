using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;  //自定义类库.

namespace GameServer.Controller
{
    abstract class BaseController
    {
        RequestCode requestCode = RequestCode.None;

        public abstract void DefaultHandle();
    }
}
