using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;

public class ClientManager : BaseManager {

    private Socket clientSocket;
    private const string IP = "127.0.0.1";
    private const int PORT = 6688;

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
            Debug.LogWarning("Connecting Server Error:" + e);
        }
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
            Debug.LogWarning("Closing Connection with Server Error:" + e);
        }
    }
}
