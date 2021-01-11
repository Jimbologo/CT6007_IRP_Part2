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
            Debug.Log("[CORRECT]: Block is Valid, now adding to blockchain");
            blockchain.Add(a_blockToValidate);

            //TODO: SEND UPDATED BLOCKCHAIN TO ALL USERS AND MINERS
            FindObjectOfType<User>().UpdateBlockchain(blockchain);
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

    public void DebugBlockchain()
    {
        Debug.Log("-----------------------------------------");
        Debug.Log("Genesis Block Data: ");
        Debug.Log("Genesis Block Previous Hash: " + System.Text.Encoding.Default.GetString(blockchain[0].GetPreviousBlockHash()));
        Debug.Log("Genesis Block Current Hash: " + System.Text.Encoding.Default.GetString(blockchain[0].GetCurrentBlockHash()));

        for (int i = 1; i < blockchain.Count; ++i)
        {
            Debug.Log("-----------------------------------------");
            Debug.Log("Block " + i + " Data: ");
            Debug.Log("Block " + i + " Previous Hash: " + System.Text.Encoding.Default.GetString(blockchain[i].GetPreviousBlockHash()));
            Debug.Log("Block " + i + " Current Hash: " + System.Text.Encoding.Default.GetString(blockchain[i].GetCurrentBlockHash()));
        }
    }
}
