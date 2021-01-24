using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;

/// <summary>
/// Client Network Management
/// </summary>
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

        //Call inialise host when Gameobject is active
        InitaliseHost();
    }

    /// <summary>
    /// Returns active state
    /// </summary>
    /// <returns></returns>
    public bool isActive()
    {
        return active;
    }

    /// <summary>
    /// Start the TCP Client and ready to connect
    /// </summary>
    private void InitaliseHost()
    {
        //Creates IPAddress from String
        IPAddress ipAddress = IPAddress.Parse(ipInputField.text);

        //Initalise TCP Client
        tcpClient = new TcpClient();

        //connect TCP cLIENT
        tcpClient.Connect(ipAddress, port);

        if(tcpClient.Connected){
            Debug.Log("Connected");
        }

        //Send handshake message
        SendNetMessage("Handshake");

        //Start a new thread for recieveing data from host
        Thread streamReading = new Thread(new ThreadStart(RecieveData));
        streamReading.Start();
    }

    /// <summary>
    /// Sends String messages
    /// </summary>
    /// <param name="a_smessage">String Message</param>
    public void SendNetMessage(string a_smessage)
    {
        //Write data to byte Array
        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = NET_HandleData.WriteData(a_smessage);
        //Get clients stream
        NetworkStream stream = tcpClient.GetStream();
        //send message to host
        stream.Write(byteMsg, 0, byteMsg.Length);
    }

    /// <summary>
    /// Sends int messages
    /// </summary>
    /// <param name="a_imessage"></param>
    public void SendNetMessage(int a_imessage)
    {
        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = NET_HandleData.WriteData(a_imessage);
        NetworkStream stream = tcpClient.GetStream();
        stream.Write(byteMsg, 0, byteMsg.Length);
    }

    /// <summary>
    /// Sends Block messages
    /// </summary>
    /// <param name="a_bmessage"></param>
    public void SendNetMessage(Block a_bmessage)
    {
        Debug.Log("Going to send block message");
        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = NET_HandleData.WriteData(a_bmessage);
        NetworkStream stream = tcpClient.GetStream();
        stream.Write(byteMsg, 0, byteMsg.Length);
    }

    /// <summary>
    /// Sends Blockchain messages
    /// </summary>
    /// <param name="a_bcmessage"></param>
    public void SendNetMessage(Blockchain a_bcmessage)
    {
        byte[] byteMsg = new byte[NET_Constants.packetSize];
        byteMsg = NET_HandleData.WriteData(a_bcmessage);
        NetworkStream stream = tcpClient.GetStream();
        stream.Write(byteMsg, 0, byteMsg.Length);
        Debug.Log("Blockchain message sent");
    }

    /// <summary>
    /// On a new thread, recieves data from clients
    /// </summary>
    private void RecieveData()
    {
        byte[] byteArray = new byte[NET_Constants.packetSize];
        while (tcpClient.Connected)
        {
            //Get stream of this client
            NetworkStream stream = tcpClient.GetStream();

            //Read and block until server sends a message
            if (stream != null && stream.Read(byteArray, 0, NET_Constants.packetSize) > 0)
            {
                HandleData(byteArray);
            }
        }
    }

    /// <summary>
    /// Handles data that has been read
    /// </summary>
    /// <param name="a_incomingMessage"></param>
    public void HandleData(byte[] a_incomingMessage)
    {
        Debug.Log("Host Sent data...");

        //Null check networkmananger
        if (!networkManager)
        {
            //find the component from scene before send to static method
            Debug.LogWarning("Network Manager not found in scene, locating...");
            networkManager = FindObjectOfType<NET_NetworkManager>();
        }

        NET_HandleData.ReadData(a_incomingMessage, networkManager);
    }

    /// <summary>
    /// Call to close the client connection
    /// </summary>
    private void CloseNet()
    {
        tcpClient.Close();
    }
}
