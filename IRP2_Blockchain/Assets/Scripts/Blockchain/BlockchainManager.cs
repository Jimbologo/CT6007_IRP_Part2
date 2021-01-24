using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the blockchain data 
/// </summary>
public class BlockchainManager : MonoBehaviour
{
    //Here we will setup functions for adding to the blockchain
    //We can store the blockchain here too
    //We will also need to setup genesis Block here too

    public List<Block> blockchain = new List<Block>();

    [SerializeField]
    private bool isActive = false;

    [SerializeField]
    private BlockchainDebugging blockchainDebugging;

    [SerializeField]
    private PlayerDataManager playerDataManager;

    /// <summary>
    /// Inital Awake called on first frame
    /// </summary>
    private void Awake()
    {
        //Create the inital block, genesis Block
        Block genesisBlock = new Block(new byte[] { }, new Transaction[] { new Transaction(0, 0, new Action(ActionType.GiveCoins,"Gensis Block", 0)) });
        //Add genesis block to the blockchain
        blockchain.Clear();
        blockchain.Add(genesisBlock);

        if (!blockchainDebugging)
        {
            blockchainDebugging = FindObjectOfType<BlockchainDebugging>();
        }

        if (!playerDataManager)
        {
            playerDataManager = FindObjectOfType<PlayerDataManager>();
        }

        isActive = true;
    }

    /// <summary>
    /// Gets active state
    /// </summary>
    /// <returns></returns>
    public bool GetActive()
    {
        return isActive;
    }

    /// <summary>
    /// Clears old chain and set it to new chain
    /// </summary>
    /// <param name="a_newBlockchain"></param>
    public void UpdateBlockchain(List<Block> a_newBlockchain)
    {
        blockchain.Clear();
        blockchain.AddRange(a_newBlockchain);

        //Update the UI representing blockchain data
        blockchainDebugging.UpdateBlockchainData(a_newBlockchain);

        playerDataManager.CalculateBlockchain(a_newBlockchain);

    }

    /// <summary>
    /// Debugs blockchain in console
    /// </summary>
    [System.Obsolete("Deprecated due to blockchains ability to be visually represented in UI")]
    public void DebugBlockchain()
    {
        //Debug the genesis block
        Debug.Log("-----------------------------------------");
        Debug.Log("Genesis Block Data: ");
        Debug.Log("Genesis Block Previous Hash: " + System.Text.Encoding.Default.GetString(blockchain[0].GetPreviousBlockHash()));
        Debug.Log("Genesis Block Current Hash: " + System.Text.Encoding.Default.GetString(blockchain[0].GetCurrentBlockHash()));

        //Loop and debug blocks in the chain
        for (int i = 1; i < blockchain.Count; ++i)
        {
            Debug.Log("-----------------------------------------");
            Debug.Log("Block " + i + " Data: ");
            Debug.Log("Block " + i + " Previous Hash: " + System.Text.Encoding.Default.GetString(blockchain[i].GetPreviousBlockHash()));
            Debug.Log("Block " + i + " Current Hash: " + System.Text.Encoding.Default.GetString(blockchain[i].GetCurrentBlockHash()));
        }
    }
}
