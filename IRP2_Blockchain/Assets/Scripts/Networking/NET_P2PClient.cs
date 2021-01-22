using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;

public class NET_P2PClient : MonoBehaviour
{
    private int port = 25667;

    private TcpClient tcpClient;

    [SerializeField]
    private TMP_InputField ipInputField = null;

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
    private void InitaliseHost()
    {
        //Creates IPAddress from String
        IPAddress ipAddress = IPAddress.Parse(ipInputField.text);

        //Initalise TCP Client
        tcpClient = new TcpClient();

        //connect TCP cLIENT
        tcpClient.Connect(ipAddress, port);

        if(tcpClient.Connected){
            Debug.LogError("Connected");
        }

        SendNetMessage("Handshake");

        Thread streamReading = new Thread(new ThreadStart(RecieveData));
        streamReading.Start();
    }

    public void SendNetMessage(string a_smessage)
    {
        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = NET_HandleData.WriteData(a_smessage);
        NetworkStream stream = tcpClient.GetStream();
        stream.Write(byteMsg, 0, byteMsg.Length);
        //stream.Close();
    }

    public void SendNetMessage(int a_imessage)
    {
        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = NET_HandleData.WriteData(a_imessage);
        NetworkStream stream = tcpClient.GetStream();
        stream.Write(byteMsg, 0, byteMsg.Length);
        //stream.Close();
    }

    public void SendNetMessage(Block a_bmessage)
    {
        Debug.LogError("Going to send block message");
        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = NET_HandleData.WriteData(a_bmessage);
        NetworkStream stream = tcpClient.GetStream();
        stream.Write(byteMsg, 0, byteMsg.Length);
        //stream.Close();
    }

    public void SendNetMessage(Blockchain a_bcmessage)
    {
        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = NET_HandleData.WriteData(a_bcmessage);
        NetworkStream stream = tcpClient.GetStream();
        stream.Write(byteMsg, 0, byteMsg.Length);
        //stream.Close();
        Debug.LogError("Blockchain message sent");
    }

    private void RecieveData()
    {
        byte[] byteArray = new byte[NET_Constants.packetSize];
        while (tcpClient.Connected)
        {
            
            NetworkStream stream = tcpClient.GetStream();

            if (stream != null && stream.Read(byteArray, 0, NET_Constants.packetSize) > 0)
            {
                HandleData(byteArray);
            }
        }
    }

    public void HandleData(byte[] a_incomingMessage)
    {
        Debug.LogError("Host Sent data...");

        //Null check networkmananger
        if (!networkManager)
        {
            //find the component from scene before send to static method
            Debug.LogError("Network Manager not found in scene, locating...");
            networkManager = FindObjectOfType<NET_NetworkManager>();
        }

        NET_HandleData.ReadData(a_incomingMessage, networkManager);
    }

    private void CloseNet()
    {
        tcpClient.Close();
    }
}
