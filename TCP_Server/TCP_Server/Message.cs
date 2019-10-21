using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//服务器用于处理数据头和具体数据的类.
namespace TCP_Server
{
    class Message
    {
        private byte[] data = new byte[1024];
        private int startIndex = 0;  //字节序列中存储数据的下标索引.

        ///读取一定字节数据后更新startIndex索引.
        public void AddCount(int count)
        {
            startIndex += count;
        }

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

        public int RemainSize  //用于限制可读取的最大字节数.
        {
            get
            {
                return data.Length - startIndex;
            }          
        }

        /// <summary>
        /// 解析/读取数据.
        /// </summary>
        public void ReadMessage()
        {
            while(true)
            {
                if (startIndex <= 4) return;
                int count = BitConverter.ToInt32(data, 0);  //读取由前四个字节(为一个int32)组成的数据头，即真实数据的字节数.
                if ((startIndex - 4) >= count)
                {
                    string s = Encoding.UTF8.GetString(data, 4, count);  //从4开始往后读count个字节，转换为真实数据.
                    Console.WriteLine("解析到的数据:" + s);
                    Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                    startIndex -= count + 4;
                }
                else
                {
                    break;
                }
            }           
        }
    }
}
