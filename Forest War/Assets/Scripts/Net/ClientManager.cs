using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using Common;

/// <summary>
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
        }
        catch (Exception e)
        {
            Debug.LogWarning("[ERROR]:Fail to connect server:" + e);
        }
    }

    private void Start()
    {
        clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            int count = clientSocket.EndReceive(ar);
            msg.ReadMessage(count, OnProcessMessageCallback);
            Start();
        }
        catch(Exception e)
        {
            Debug.Log("[Receive & Process message error]:" + e);
        }
    }

    private void OnProcessMessageCallback(RequestCode requestCode, string data)
    {
        gameFacade.HandleResponse(requestCode, data);
    }

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
            Debug.LogWarning("[ERROR]:Fail to close connection with Server:" + e);
        }
    }
}
