using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;


[System.Serializable]
public class NET_ConnectedClient
{
    public TcpClient tcpClient;
    public Socket connectedSocket;
    public int iID;

    public NET_ConnectedClient(Socket a_socket, int a_id)
    {
        connectedSocket = a_socket;
        iID = a_id;
    }

    public NET_ConnectedClient(TcpClient a_tcpClient, int a_id)
    {
        tcpClient = a_tcpClient;
        iID = a_id;
    }

    
}
