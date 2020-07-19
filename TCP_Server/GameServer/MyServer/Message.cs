using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.MyServer
{
    class Message
    {
        private byte[] data = new byte[1024];
        private int startIndex = 0;  //数组中存储数据的字节数.

        //get only
        public byte[] Data
        {
            get
            {
                return data;
            }
        }
        public int StartIndex
        {
            get
            {
                return startIndex;
            }
        }
        public int RemainSize  //可读取的最大字节数.
        {
            get
            {
                return data.Length - startIndex;
            }
        }

        /// <summary>
        /// 解析/读取数据，处理粘包分包问题.
        /// </summary>
        /// <param name="newDataAmount">接收到的原始数据</param>
        /// <param name="processDataCallback">处理解析数据的回调方法</param>
        public void ReadMessage(int newDataAmount, Action<RequestCode,ActionCode,string> processDataCallback)
        {
            startIndex += newDataAmount;
            while (true)
            {
                if (startIndex <= 4) return;  //数据头不完整 或 粘包的所有数据处理完毕且数据缓存为空.
                int count = BitConverter.ToInt32(data, 0);

                if ((startIndex - 4) >= count)  
                {
                    //==时表示实际数据和数据头count是一致的，循环只执行一次.
                    //>时表示可能发生了粘包，此时循环体尝试区分各个包的实际内容.

                    RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, 4);
                    ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 8);
                    string strData = Encoding.UTF8.GetString(data, 12, count - 8);
                    processDataCallback(requestCode, actionCode, strData);
                    Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                    startIndex -= count + 4;
                }
                else  //当前数据不完整.
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 打包需要返回给客户端的数据，由数据长度、RequestCode和真实数据组成.
        /// </summary>
        /// <param name="actionCode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] PackResponseData(ActionCode actionCode, string data)
        {
            byte[] requestCodeBytes = BitConverter.GetBytes((int)actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataAmount = requestCodeBytes.Length + dataBytes.Length;
            byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
            byte[] newBytes = dataAmountBytes.Concat(requestCodeBytes).ToArray();
            return newBytes.Concat(dataBytes).ToArray();
            //return dataAmountBytes.Concat(requestCodeBytes).Concat(dataBytes).ToArray();
        }
    }
}
