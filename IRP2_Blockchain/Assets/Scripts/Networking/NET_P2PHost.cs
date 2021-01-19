using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class NET_P2PHost : MonoBehaviour
{
    private int maxPlayers = 0;
    private int hostPort = 25667;


    private static TcpListener tcpListener;
    private bool tcpListenerActive = false;

    private List<NET_ConnectedClient> connectedClients = new List<NET_ConnectedClient>();

    private void Start()
    {
        InitaliseHost();

        Thread streamReading = new Thread(new ThreadStart(RecieveMessage));
        streamReading.Start();
    }

    //Here we start the TCP Lister and ready to connect clients
    private void InitaliseHost()
    {
        //Creates IPAddress from String
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

        //Initalise TCP Listener
        tcpListener = new TcpListener(ipAddress, hostPort);
        tcpListener.Start();
        tcpListenerActive = true;

        Debug.Log("Host running on port: " + hostPort);
        Debug.Log("Host local end point: " + tcpListener.LocalEndpoint);
        Debug.Log("Waiting on a connection...");

        //Setup Socket
        tcpListener.BeginAcceptSocket(
        new System.AsyncCallback(SocketCallback), tcpListener);
    }

    private void SocketCallback(System.IAsyncResult a_iAsyncResult)
    {
        TcpListener clientTCPlistener = (TcpListener)a_iAsyncResult.AsyncState;
        Socket clientSocket = clientTCPlistener.EndAcceptSocket(a_iAsyncResult);

        Debug.Log("Client Connection Accepted from: " + clientSocket.RemoteEndPoint);

        //Add client to the connected list
        connectedClients.Add(new NET_ConnectedClient(clientSocket, connectedClients.Count));

        SendMessage(clientSocket, "Welcome Suzan");
    }

    public void SendMessage(Socket a_clientSocket, string a_smessage)
    {
        byte[] byteMsg = Encoding.ASCII.GetBytes(a_smessage);
        a_clientSocket.Send(byteMsg);
    }

    private void RecieveMessage()
    {
        byte[] byteArray = new byte[100];

        while (tcpListenerActive)
        { 
            for (int i = 0; i < connectedClients.Count; ++i)
            {
                if (connectedClients[i] != null && connectedClients[i].connectedSocket != null && connectedClients[i].connectedSocket.Receive(byteArray) > 0)
                {
                    HandleData(byteArray);
                }
            }
        }
    }

    private void HandleData(byte[] a_incomingMessage)
    {
        Debug.Log("Client Sent a Message: " + System.Text.Encoding.Default.GetString(a_incomingMessage));


    }

    private void CloseNet()
    {
        tcpListener.Stop();
        tcpListenerActive = false;
    }

}
