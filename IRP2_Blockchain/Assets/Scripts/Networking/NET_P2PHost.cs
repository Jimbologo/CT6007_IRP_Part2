using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Open.Nat;

public class NET_P2PHost : MonoBehaviour
{
    private int hostPort = 25667;

    private static TcpListener tcpListener;
    private bool tcpListenerActive = false;

    private List<NET_ConnectedClient> connectedClients = new List<NET_ConnectedClient>();

    private bool active = false;

    private void Start()
    {
        active = true;

        InitaliseHost();
    }

    public bool isActive()
    {
        return active;
    }

    //Here we start the TCP Lister and ready to connect clients
    private async void InitaliseHost()
    {
        //Setup UPnP Port mapping
        await upnpAsync();

        //Creates IPAddress from String
        IPAddress ipAddress = IPAddress.Any;

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

        Thread streamReading = new Thread(new ThreadStart(RecieveData));
        streamReading.Start();
    }

    private async System.Threading.Tasks.Task upnpAsync()
    {
        var discoverer = new NatDiscoverer();

        // using SSDP protocol to discovers NAT device.
        var device = await discoverer.DiscoverDeviceAsync();

        // debug the NAT IP address
        Debug.Log($"The external IP Address is: " + (await device.GetExternalIPAsync()).ToString());

        // create a new mapping in the router
        await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, hostPort, hostPort, "Blockchain"));

        foreach (var mapping in await device.GetAllMappingsAsync())
        {
            Debug.Log(mapping);
        }

        Debug.Log("Port mapping by UPnP complete");
    }

    private void SocketCallback(System.IAsyncResult a_iAsyncResult)
    {
        TcpListener clientTCPlistener = (TcpListener)a_iAsyncResult.AsyncState;
        Socket clientSocket = clientTCPlistener.EndAcceptSocket(a_iAsyncResult);

        Debug.Log("Client Connection Accepted from: " + clientSocket.RemoteEndPoint);

        //Add client to the connected list
        connectedClients.Add(new NET_ConnectedClient(clientSocket, connectedClients.Count));

        SendNetMessage(connectedClients.Count, "Welcome Client");
    }

    public void SendNetMessageToAll(string a_smessage)
    {
        byte[] byteMsg = Encoding.ASCII.GetBytes(a_smessage);

        Socket targetSocket = null;
        for (int i = 0; i < connectedClients.Count; ++i)
        {
            targetSocket = connectedClients[i].connectedSocket;

            if (targetSocket != null)
            {
                targetSocket.Send(byteMsg);
            }
        }
    }

    public void SendNetMessage(int a_clientID, string a_smessage)
    {
        byte[] byteMsg = Encoding.ASCII.GetBytes(a_smessage);


        Socket targetSocket = null;
        for (int i = 0; i < connectedClients.Count; ++i)
        {
            if(connectedClients[i].iID == a_clientID)
            {
                targetSocket = connectedClients[i].connectedSocket;
            }
        }

        if (targetSocket != null)
        {
            targetSocket.Send(byteMsg);
        }
    }

    private void RecieveData()
    {
        byte[] byteArray = new byte[100];

        while (tcpListenerActive)
        { 
            for (int i = 0; i < connectedClients.Count; ++i)
            {
                if (connectedClients[i] != null && connectedClients[i].connectedSocket != null &&
                    connectedClients[i].connectedSocket.Connected && connectedClients[i].connectedSocket.Receive(byteArray) > 0)
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
