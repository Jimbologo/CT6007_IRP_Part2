using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        //Create the inital block, genesis Block
        Block genesisBlock = new Block(new byte[] { }, new Transaction[] { new Transaction(0, 0, new Action(ActionType.GiveCoins,"Gensis Block", 0)) });
        //Add genesis block to the blockchain
        blockchain.Clear();
        blockchain.Add(genesisBlock);

        isActive = true;
    }

    public bool GetActive()
    {
        return isActive;
    }

    //Clears old chain and set it to new chain
    public void UpdateBlockchain(List<Block> a_newBlockchain)
    {
        blockchain.Clear();
        blockchain.AddRange(a_newBlockchain);

        //Update the UI representing blockchain data
        blockchainDebugging.UpdateBlockchainData(a_newBlockchain);

        playerDataManager.CalculateBlockchain(a_newBlockchain);
    }

    public void DebugBlockchain()
    {
        Debug.LogError("-----------------------------------------");
        Debug.LogError("Genesis Block Data: ");
        Debug.LogError("Genesis Block Previous Hash: " + System.Text.Encoding.Default.GetString(blockchain[0].GetPreviousBlockHash()));
        Debug.LogError("Genesis Block Current Hash: " + System.Text.Encoding.Default.GetString(blockchain[0].GetCurrentBlockHash()));

        for (int i = 1; i < blockchain.Count; ++i)
        {
            Debug.LogError("-----------------------------------------");
            Debug.LogError("Block " + i + " Data: ");
            Debug.LogError("Block " + i + " Previous Hash: " + System.Text.Encoding.Default.GetString(blockchain[i].GetPreviousBlockHash()));
            Debug.LogError("Block " + i + " Current Hash: " + System.Text.Encoding.Default.GetString(blockchain[i].GetCurrentBlockHash()));
        }
    }
}
