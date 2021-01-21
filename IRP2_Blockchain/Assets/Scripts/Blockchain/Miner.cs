using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Miner : BlockchainManager
{
    //Use this to validate Is a block that has been added

    //Data from Messages from P2P will be sent into this class to manage mining of the blockchain
    //Such as if a new block is wanting to be added, the block data and a list of transactions that have taken place will be sent here
    //That block will be validated, once validated the blockchain will be updated and passed around

    private NET_NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = FindObjectOfType<NET_NetworkManager>();
    }

    //Validates a new block
    public void ValidateBlock(Transaction[] a_transactionsToValidate,Block a_blockToValidate)
    {
        //We need to calculate what the hash would be of the new transactions, based on this data stored
        //We then compare that hash with the one sent to check if they are the same, if they are, we class this block as validated

        //get latest block in blockchain
        Block lastBlockInBlockchain = blockchain[blockchain.Count - 1];
        //Calculate the hash the latest block should have
        byte[] hashOfTransactions = Block.CalculateHashReturned(a_transactionsToValidate, lastBlockInBlockchain.GetCurrentBlockHash());
        
        //Check if the hashes are the same
        if (hashOfTransactions.SequenceEqual(a_blockToValidate.GetCurrentBlockHash()))
        {
            Debug.LogError("[CORRECT]: Block is Valid, now adding to blockchain");
            blockchain.Add(a_blockToValidate);

            //SEND UPDATED BLOCKCHAIN TO ALL USERS AND MINERS
            Blockchain newBlockchain = new Blockchain();
            newBlockchain.theBlockchain = blockchain;

            networkManager.SendNetMessage(newBlockchain, -1);
        }
        else
        {
            Debug.LogWarning("[FAILURE]: Block is NOT Valid");
        }
    }

    //This would normally be a message sent from a user not this function
    public void AddToBlockchain(Block a_blockToAdd)
    {
        ValidateBlock(a_blockToAdd.GetTransactions(), a_blockToAdd);
    }

    
}
