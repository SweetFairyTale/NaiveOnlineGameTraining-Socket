using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Client
{
    //组拼数据头和真实数据，形成一个完整字节序列.
    class Message
    {
        public static byte[] GetBytes(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);  //数据
            int dataLength = dataBytes.Length;
            byte[] bytesLength = BitConverter.GetBytes(dataLength);  //数据头(数据长度)
            byte[] newBytes = bytesLength.Concat(dataBytes).ToArray();
            return newBytes;
        }
    }
}
