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

    NET_NetworkManager networkManager;

    private void Start()
    {
        networkManager = FindObjectOfType<NET_NetworkManager>();

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

        Thread streamReading = new Thread(new ThreadStart(RecieveData));
        streamReading.Start();

        Thread socketAccepting = new Thread(new ThreadStart(SocketAccepting));
        socketAccepting.Start();
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

    private void SocketAccepting()
    {
        while (tcpListenerActive)
        {
            //Setup Socket
            Socket clientSocket = tcpListener.AcceptSocket();

            //Turn blocking off so we can easily loop through all clients later (VERY IMPORTANT otherwise only 1 client will be able to send messages at once)
            //clientSocket.Blocking = false;

            Debug.Log("Client Connection Accepted from: " + clientSocket.RemoteEndPoint);

            //Add client to the connected list
            int newID = connectedClients.Count;
            connectedClients.Add(new NET_ConnectedClient(clientSocket, newID));

            SendNetMessage(newID, "Welcome Client");

            //update every users client list
            List<int> clients = new List<int>();

            for (int i = 0; i < connectedClients.Count; ++i)
            {
                clients.Add(connectedClients[i].iID);
            }
            SendNetMessageToAll(clients);
            networkManager.HandleClientListData(clients);
        }
    }

    #region Send Message Functions

    public void SendNetMessageToAll(string a_smessage)
    {
        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = Encoding.ASCII.GetBytes(a_smessage);

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

    public void SendNetMessageToAll(int a_imessage)
    {
        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = NET_HandleData.WriteData(a_imessage);

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

    public void SendNetMessageToAll(Block a_bmessage)
    {
        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = NET_HandleData.WriteData(a_bmessage);

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

    public void SendNetMessageToAll(Blockchain a_bcmessage)
    {
        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = NET_HandleData.WriteData(a_bcmessage);

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

    public void SendNetMessageToAll(List<int> a_connectedClients)
    {
        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = NET_HandleData.WriteData(a_connectedClients);

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
        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = NET_HandleData.WriteData(a_smessage);

        Socket targetSocket = null;
        for (int i = 0; i < connectedClients.Count; ++i)
        {
            if (connectedClients[i].iID == a_clientID)
            {
                targetSocket = connectedClients[i].connectedSocket;
            }
        }

        if (targetSocket != null)
        {
            targetSocket.Send(byteMsg);
        }
    }

    public void SendNetMessage(int a_clientID, int a_imessage)
    {
        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = NET_HandleData.WriteData(a_imessage);

        Socket targetSocket = null;
        for (int i = 0; i < connectedClients.Count; ++i)
        {
            if (connectedClients[i].iID == a_clientID)
            {
                targetSocket = connectedClients[i].connectedSocket;
            }
        }

        if (targetSocket != null)
        {
            targetSocket.Send(byteMsg);
        }
    }

    public void SendNetMessage(int a_clientID, Block a_bmessage)
    {
        if(a_clientID == -1)
        {
            SendNetMessageToAll(a_bmessage);
            return;
        }

        Debug.LogError("Going to send block message");

        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = NET_HandleData.WriteData(a_bmessage);

        Socket targetSocket = null;
        for (int i = 0; i < connectedClients.Count; ++i)
        {
            if (connectedClients[i].iID == a_clientID)
            {
                targetSocket = connectedClients[i].connectedSocket;
            }
        }

        if (targetSocket != null)
        {
            targetSocket.Send(byteMsg);
            Debug.LogError("Sending block message");
        }
        else
        {
            Debug.LogError("targetSocket invalid");
        }
    }

    public void SendNetMessage(int a_clientID, Blockchain a_bcmessage)
    {
        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = NET_HandleData.WriteData(a_bcmessage);

        Socket targetSocket = null;
        for (int i = 0; i < connectedClients.Count; ++i)
        {
            if (connectedClients[i].iID == a_clientID)
            {
                targetSocket = connectedClients[i].connectedSocket;
            }
        }

        if (targetSocket != null)
        {
            targetSocket.Send(byteMsg);
        }
    }

    #endregion

    private void RecieveData()
    {
        byte[] byteArray = new byte[NET_Constants.packetSize];

        while (tcpListenerActive)
        { 
            //loop through each 
            for (int i = 0; i < connectedClients.Count; ++i)
            {
                
                //Lets check that when we recieve it contains data
                if (connectedClients[i] != null 
                    && connectedClients[i].connectedSocket != null
                    && connectedClients[i].connectedSocket.Connected 
                    && connectedClients[i].connectedSocket.Available > 0
                    && connectedClients[i].connectedSocket.Receive(byteArray) > 0)
                {
                    //Take the data from that was recieve and lets process it
                    HandleData(byteArray);
                }
            }
        }
    }

    private void HandleData(byte[] a_incomingMessage)
    {
        Debug.Log("Client Sent a Message");

        //Null check networkmananger
        if(!networkManager)
        {
            //find the component from scene before send to static method
            Debug.LogError("Network Manager not found in scene, locating...");
            networkManager = FindObjectOfType<NET_NetworkManager>();
        }

        NET_HandleData.ReadData(a_incomingMessage, networkManager);
    }

    private void CloseNet()
    {
        tcpListener.Stop();
        tcpListenerActive = false;
    }

}
