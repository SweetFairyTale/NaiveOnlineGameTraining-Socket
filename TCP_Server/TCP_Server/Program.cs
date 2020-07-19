/*
 * proj 001 -- 控制台应用服务器端测试.
 */
using System;
using System.Text;
using System.Net.Sockets;  //引入Socket相关命名空间.
using System.Net;

namespace TCP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            StartServerAsync();
            Console.ReadKey();
        }

        //static byte[] dataBuffer = new byte[1024];  //该数据缓冲已转移至Message类.

        //异步接收消息方法.
        static void StartServerAsync()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse("172.16.59.6");  //公网IP:47.100.2.223  内网IP:172.16.59.6
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 10001);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(10);
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }
        static Message msg = new Message();  //自定义消息处理对象.

        static void AcceptCallBack(IAsyncResult ar)
        {
            Socket serverSocket = ar.AsyncState as Socket;
            Socket clientSocket = serverSocket.EndAccept(ar);

            //向客户端发送一条消息.
            string msgSrt = "hello,你好。";
            byte[] data = Encoding.UTF8.GetBytes(msgSrt);
            clientSocket.Send(data);

            //接收客户端的多次消息发送.            
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, clientSocket);
            //等待下一个客户端的连接.
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }

        static void ReceiveCallBack(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                int count = clientSocket.EndReceive(ar); //EndReceive遇到客户端非正常关闭时抛出异常.
                if (count == 0)
                {
                    //当客户端正常关闭时，EndReceive会持续受到长度为0的数据，此时根据count为0即可进行处理.
                    //当客户端发送空消息时，服务器不会接收到任何消息(conut无值).
                    clientSocket.Close();  //正常关闭
                    
                    return;
                }

                msg.AddCount(count); //!

                //string msgStr = Encoding.UTF8.GetString(dataBuffer, 0, count);
                //Console.WriteLine("从客户端异步接收的数据：" + msgStr);

                msg.ReadMessage();
                //clientSocket.BeginReceive(dataBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, clientSocket);  //循环回调.
                clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, clientSocket);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                if(clientSocket != null)
                {
                    clientSocket.Close();  //非正常关闭.
                }
            }    
            
        }

        ////同步接收消息方法 -- 程序会阻塞
        //static void StartServerSync()
        //{
        //    Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    IPAddress ipAddress = IPAddress.Parse("127.0.0.1");  //47.100.2.223
        //    IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 2049);
        //    serverSocket.Bind(ipEndPoint);  //绑定ip与端口号.
        //    serverSocket.Listen(10);  //同时等待处理的队列长度(不是最大玩家数).
        //    Socket clientSocket = serverSocket.Accept();  //接收一个客户端连接.

        //    //向客户端发送一条消息.
        //    string msg = "hello, 你好。";
        //    byte[] data = Encoding.UTF8.GetBytes(msg);
        //    clientSocket.Send(data);

        //    //接收客户端的一条消息.
        //    //byte[] dataBuffer = new byte[1024];
        //    int count = clientSocket.Receive(dataBuffer);
        //    string msgReceive = Encoding.UTF8.GetString(dataBuffer, 0, count);

        //    Console.WriteLine(msgReceive);

        //    clientSocket.Close();
        //    serverSocket.Close();
        //}
    }
}
