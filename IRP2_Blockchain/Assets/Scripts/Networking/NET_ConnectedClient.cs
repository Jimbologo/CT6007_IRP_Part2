using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// Holds Client ID and socket data for hosts
/// </summary>
[System.Serializable]
public class NET_ConnectedClient
{
    public TcpClient tcpClient;
    public Socket connectedSocket;
    public int iID;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="a_socket"></param>
    /// <param name="a_id"></param>
    public NET_ConnectedClient(Socket a_socket, int a_id)
    {
        connectedSocket = a_socket;
        iID = a_id;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="a_tcpClient"></param>
    /// <param name="a_id"></param>
    public NET_ConnectedClient(TcpClient a_tcpClient, int a_id)
    {
        tcpClient = a_tcpClient;
        iID = a_id;
    }

    
}
