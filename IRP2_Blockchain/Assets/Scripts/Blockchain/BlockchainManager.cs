using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockchainManager : MonoBehaviour
{
    //Here we will setup functions for adding to the blockchain
    //We can store the blockchain here too
    //We will also need to setup genesis Block here too

    protected List<Block> blockchain = new List<Block>();

    private void Awake()
    {
        //Create the inital block, genesis Block
        Block genesisBlock = new Block(new byte[] { }, new Transaction[] { new Transaction(0, 0, "abc") });
        //Add genesis block to the blockchain
        blockchain.Clear();
        blockchain.Add(genesisBlock);

        
    }
}
