using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Network Constants
/// </summary>
static class NET_Constants
{
    public const int packetSize = 20480; //20KB
}

/// <summary>
/// Call point for Users and Miners to send and handle NET Data
/// </summary>
public class NET_NetworkManager : MonoBehaviour
{
    [SerializeField]
    private NET_P2PClient p2pClient = null;
    [SerializeField]
    private NET_P2PHost p2pHost = null;

    [SerializeField]
    private Miner miner = null;
    [SerializeField]
    private User user = null;

    [SerializeField]
    private UserActionPanel userActionPanel = null;

    [SerializeField]
    private PlayerDataManager playerDataManager = null;

    public int myID = -1;

    /// <summary>
    /// Called on first frame
    /// </summary>
    private void Awake()
    {
        Screen.SetResolution(1000,800,false);
    }

    #region Sending Net Messages

    /// <summary>
    /// Send Network Message
    /// </summary>
    /// <param name="a_newMessage"></param>
    /// <param name="a_clientID"></param>
    public void SendNetMessage(string a_newMessage, int a_clientID)
    {
        //Check wether we are host of client then call send
        if (p2pClient && p2pClient.isActive())
        {
            p2pClient.SendNetMessage(a_newMessage);
        }
        else if (p2pHost && p2pHost.isActive())
        {
            p2pHost.SendNetMessage(a_clientID,a_newMessage);
        }
    }

    /// <summary>
    /// Send Network Message
    /// </summary>
    /// <param name="a_newMessage"></param>
    /// <param name="a_clientID"></param>
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

    /// <summary>
    /// Send Network Message
    /// </summary>
    /// <param name="a_newMessage"></param>
    /// <param name="a_clientID"></param>
    public void SendNetMessage(Block a_newMessage, int a_clientID)
    {
        if (p2pClient && p2pClient.isActive())
        {
            Debug.Log("Sending block data from Client");
            p2pClient.SendNetMessage(a_newMessage);
        }
        else if (p2pHost && p2pHost.isActive())
        {
            Debug.Log("Sending block data from Host");
            p2pHost.SendNetMessage(a_clientID, a_newMessage);
        }
        else
        {
            Debug.LogWarning("Host or Client was not found when sending message... are the in the scene?");
        }
    }

    /// <summary>
    /// Send Network Message
    /// </summary>
    /// <param name="a_newMessage"></param>
    /// <param name="a_clientID"></param>
    public void SendNetMessage(Blockchain a_newMessage, int a_clientID)
    {
        if (p2pClient && p2pClient.isActive())
        {
            Debug.Log("Sending blockchain data from Client");
            p2pClient.SendNetMessage(a_newMessage);
        }
        else if (p2pHost && p2pHost.isActive())
        {
            if(a_clientID == -1)
            {
                Debug.Log("Sending blockchain data to ALL from Host");
                p2pHost.SendNetMessageToAll(a_newMessage);
            }
            else
            {
                Debug.Log("Sending blockchain data from Host");
                p2pHost.SendNetMessage(a_clientID, a_newMessage);
            }
        }
        else
        {
            Debug.LogWarning("Host or Client was not found when sending message... are the in the scene?");
        }
    }

    #endregion

    #region Handle Recieved Data

    /// <summary>
    /// Handle Network Data
    /// </summary>
    /// <param name="a_blockData"></param>
    public void HandleBlockData(Block a_blockData)
    {
        //Might need to forward this to all other connected clients
        if (p2pHost && p2pHost.isActive())
        {
            Debug.Log("Trying to send block data forward to ALL");
            p2pHost.SendNetMessageToAll(a_blockData);
        }

        //call for the miner to validate the block
        if (miner.GetActive())
        {
            Debug.Log("sorting blockchain data out as miner...");
            miner.AddToBlockchain(a_blockData);
        }
        else if(!user.GetActive())
        {
            Debug.LogWarning("Block data not handled due to missing Miner in Scene");
        }
    }

    /// <summary>
    /// Handle Network Data
    /// </summary>
    /// <param name="a_blockchainData"></param>
    public void HandleBlockchainData(Blockchain a_blockchainData)
    {
        //Might need to forward this to all other connected clients
        if (p2pHost && p2pHost.isActive())
        {
            Debug.Log("Sending blockchain data to ALL!");
            p2pHost.SendNetMessageToAll(a_blockchainData);
        }

        //Call for all clients to update there version of the blockchain
        if (user.GetActive())
        {
            user.UpdateBlockchain(a_blockchainData.theBlockchain);
        }
        else if (miner.GetActive())
        {
            miner.UpdateBlockchain(a_blockchainData.theBlockchain);
        }
        else
        {
            Debug.LogWarning("User or miner not found, this is an issue!");
        }
    }

    /// <summary>
    /// Handle Network Data
    /// </summary>
    /// <param name="a_clientListData"></param>
    public void HandleClientListData(NET_ClientList a_clientListData)
    {
        userActionPanel.clientListData = a_clientListData;
        userActionPanel.updateRequired = true;
    }

    /// <summary>
    /// Handle Network Data
    /// </summary>
    /// <param name="a_clientListData"></param>
    public void HandleClientListData(List<int> a_clientListData)
    {
        userActionPanel.clientListData = new NET_ClientList(a_clientListData);
        userActionPanel.updateRequired = true;
    }

    /// <summary>
    /// Handle Network Data
    /// </summary>
    public void UpdatePlayerData()
    {
        playerDataManager.UpdateValues(myID);
    }

    #endregion
}
