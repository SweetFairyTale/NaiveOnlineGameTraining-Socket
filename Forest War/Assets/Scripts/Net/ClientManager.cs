using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using Common;

/// <summary>
/// 作为游戏资源和各个模块的中介
/// 管理当前客户端与服务器端的Socket连接.
/// </summary>
public class ClientManager : BaseManager {
   
    private const string IP = "127.0.0.1";
    private const int PORT = 6688;

    private Socket clientSocket;
    private Message msg = new Message();

    public ClientManager(GameFacade facade) : base(facade) { }

    public override void OnInit()
    {
        base.OnInit();
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(IP, PORT);
            Start();
        }
        catch (Exception e)
        {
            Debug.LogWarning("[ERROR]:Fail To Connect Server:" + e);
        }
    }

    private void Start()
    {
        clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
    }

    //△System.Net.Sockets中的BeginReceive的回调方法无法直接访问Unity中的游戏资源△
    private void ReceiveCallback(IAsyncResult ar)
    {
        Debug.Log("ReceiveCallback Called");
        try
        {
            int count = clientSocket.EndReceive(ar);
            msg.ReadMessage(count, OnProcessMessageCallback);
            Start();
        }
        catch(Exception e)
        {
            Debug.Log("[Receive & Process Message Error]:" + e);
        }
    }

    private void OnProcessMessageCallback(ActionCode actionCode, string data)
    {
        gameFacade.HandleResponse(actionCode, data);
    }

    /// <summary>
    /// 提供打包数据和发送到服务器端的方法，通过GameFacade中介来调用
    /// </summary>
    /// <param name="requestCode"></param>
    /// <param name="actionCode"></param>
    /// <param name="data"></param>
    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        byte[] bytes = Message.PackResponseData(requestCode, actionCode, data);
        clientSocket.Send(bytes);
    }


    public override void OnDestroy()
    {
        base.OnDestroy();
        try
        {
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Debug.LogWarning("[ERROR]:Fail To Close Connection With Server:" + e);
        }
    }
}
