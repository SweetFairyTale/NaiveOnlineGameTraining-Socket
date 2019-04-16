﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Server
{
    class Message
    {
        private byte[] data = new byte[1024];
        private int startIndex = 0;  //数组中存储数据的字节数.

        //public void AddCount(int count)  //交由ReadMessage完成.
        //{
        //    startIndex += count;
        //}

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
        /// 解析/读取数据.
        /// </summary>
        public void ReadMessage(int newDataAmount)
        {
            startIndex += newDataAmount;
            while (true)
            {
                if (startIndex <= 4) return;
                int count = BitConverter.ToInt32(data, 0);
                if ((startIndex - 4) >= count)
                {
                    string s = Encoding.UTF8.GetString(data, 4, count);
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