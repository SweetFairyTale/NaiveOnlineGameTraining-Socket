using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Common;
using System.Text;
using System.Linq;

public class Message 
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
    /// 客户端解析/读取数据，处理粘包分包问题.
    /// </summary>
    /// <param name="newDataAmount">接收到的原始数据长度</param>
    /// <param name="processDataCallback">处理解析数据的回调方法</param>
    public void ReadMessage(int newDataAmount, Action<ActionCode, string> processDataCallback)
    {
        startIndex += newDataAmount;
        while (true)
        {
            if (startIndex <= 4) return;
            int count = BitConverter.ToInt32(data, 0);
            if ((startIndex - 4) >= count)
            {
                ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 4);
                string strData = Encoding.UTF8.GetString(data, 8, count - 4);
                processDataCallback(actionCode, strData);  //无法return多个值所以使用回调方法直接在这儿处理？
                Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                startIndex -= count + 4;
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// 打包需要发送到服务器端的数据，由数据长度，RC，AC，和真实数据组成.
    /// </summary>
    /// <param name="requestCode"></param>
    /// <param name="actionCode"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] PackResponseData(RequestCode requestCode,ActionCode actionCode, string data)
    {
        byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
        byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        int dataAmount = requestCodeBytes.Length + dataBytes.Length + actionCodeBytes.Length;
        byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
        return dataAmountBytes.Concat(requestCodeBytes).ToArray()
            .Concat(actionCodeBytes).ToArray()
            .Concat(dataBytes).ToArray();
    }
}
