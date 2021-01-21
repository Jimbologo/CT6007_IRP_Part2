using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NET_NetworkManager : MonoBehaviour
{
    [SerializeField]
    private NET_P2PClient p2pClient;
    [SerializeField]
    private NET_P2PHost p2pHost;

    [SerializeField]
    private Miner miner;
    [SerializeField]
    private User user;

    private void Awake()
    {
        Screen.SetResolution(400,400,false);
    }

    public void SendNetMessage(string a_newMessage, int a_clientID)
    {
        if (p2pClient && p2pClient.isActive())
        {
            p2pClient.SendNetMessage(a_newMessage);
        }
        else if (p2pHost && p2pHost.isActive())
        {
            p2pHost.SendNetMessage(a_clientID,a_newMessage);
        }
    }

    public void SendNetMessage(int a_newMessage, int a_clientID)
    {
        if (p2pClient && p2pClient.isActive())
        {
            p2pClient.SendNetMessage(a_newMessage);
        }
        else if (p2pHost && p2pHost.isActive())
        {
            p2pHost.SendNetMessage(a_clientID, a_newMessage);
        }
    }

    public void SendNetMessage(Block a_newMessage, int a_clientID)
    {
        if (p2pClient && p2pClient.isActive())
        {
            Debug.LogError("Sending block data from Client");
            p2pClient.SendNetMessage(a_newMessage);
        }
        else if (p2pHost && p2pHost.isActive())
        {
            Debug.LogError("Sending block data from Host");
            p2pHost.SendNetMessage(a_clientID, a_newMessage);
        }
        else
        {
            Debug.LogError("Host or Client was not found when sending message... are the in the scene?");
        }
    }

    public void SendNetMessage(Blockchain a_newMessage, int a_clientID)
    {
        if (p2pClient && p2pClient.isActive())
        {
            p2pClient.SendNetMessage(a_newMessage);
        }
        else if (p2pHost && p2pHost.isActive())
        {
            if(a_clientID == -1)
            {
                p2pHost.SendNetMessageToAll(a_newMessage);
            }
            else
            {
                p2pHost.SendNetMessage(a_clientID, a_newMessage);
            }
        }
    }

    public void HandleBlockData(Block a_blockData)
    {
        //Might need to forward this to all other connected clients
        if (p2pHost && p2pHost.isActive())
        {
            p2pHost.SendNetMessageToAll(a_blockData);
        }

        if (miner.GetActive())
        {
            miner.AddToBlockchain(a_blockData);
        }
        else if(!user.GetActive())
        {
            Debug.LogError("Block data not handled due to missing Miner in Scene");
        }
    }

    public void HandleBlockchainData(Blockchain a_blockchainData)
    {
        //Might need to forward this to all other connected clients
        if (p2pHost && p2pHost.isActive())
        {
            p2pHost.SendNetMessageToAll(a_blockchainData);
        }

        if (user.GetActive())
        {
            user.UpdateBlockchain(a_blockchainData.theBlockchain);
        }
        if (miner.GetActive())
        {
            miner.UpdateBlockchain(a_blockchainData.theBlockchain);
        }
        else
        {
            Debug.LogError("User or miner not found, this is an issue!");
        }
    }
}
