using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NET_NetworkManager : MonoBehaviour
{
    private NET_P2PClient p2pClient;
    private NET_P2PHost p2pHost;

    private void Start()
    {
        p2pClient = GetComponentInChildren<NET_P2PClient>();
        p2pHost = GetComponentInChildren<NET_P2PHost>();
    }

    public void SendNetMessage(string a_newMessage, int a_clientID)
    {
        if(p2pClient.isActive())
        {
            p2pClient.SendNetMessage(a_newMessage);
        }
        else if (p2pHost.isActive())
        {
            p2pHost.SendNetMessage(a_clientID,a_newMessage);
        }
    }
}
