using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class NET_ConnectedClient
{
    public Socket connectedSocket;
    public int iID;

    public NET_ConnectedClient(Socket a_socket, int a_id)
    {
        connectedSocket = a_socket;
        iID = a_id;
    }
}
