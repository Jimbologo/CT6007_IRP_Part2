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

    private void Start()
    {
        active = true;

        InitaliseHost();

        Thread streamReading = new Thread(new ThreadStart(RecieveData));
        streamReading.Start();
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
        Debug.LogError("Connected");

        SendNetMessage("Handshake");

    }

    public void SendNetMessage(string a_smessage)
    {
        byte[] byteMsg = Encoding.ASCII.GetBytes(a_smessage);
        NetworkStream stream = tcpClient.GetStream();
        stream.Write(byteMsg, 0, byteMsg.Length);
    }

    private void RecieveData()
    {
        byte[] byteArray = new byte[100];
        while (tcpClient.Connected)
        {
            NetworkStream stream = tcpClient.GetStream();

            if (stream != null && stream.Read(byteArray, 0, 100) > 0)
            {
                HandleData(byteArray);
            }
        }
    }

    private void HandleData(byte[] a_incomingMessage)
    {
        Debug.LogError("Server Sent a Message: " + System.Text.Encoding.Default.GetString(a_incomingMessage));
    }

    private void CloseNet()
    {
        tcpClient.Close();
    }
}
