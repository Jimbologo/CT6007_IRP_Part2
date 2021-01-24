using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Abilty to create new transactions and add to a block ready to send across the network
/// </summary>
public class User : BlockchainManager
{
    private NET_NetworkManager networkManager;

    [SerializeField]
    private Dropdown playerSelectionDropdown = null;

    private List<Transaction> transactionsBuffer = new List<Transaction>();

    [Range(1,5)]
    [SerializeField]
    private int imaxBufferSize = 3;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        networkManager = FindObjectOfType<NET_NetworkManager>();
    }

    /// <summary>
    /// Used to determine which action to create and add to buffer
    /// </summary>
    /// <param name="actionToTaken"></param>
    public void onActionClickButton(ActionEventPass actionToTaken)
    {
        //Make sure we dont add to many transactions to the buffer
        if (transactionsBuffer.Count < imaxBufferSize)
        {
            //Create new transaction from action
            Transaction newTransaction = new Transaction(networkManager.myID, playerSelectionDropdown.value - 1, new Action(actionToTaken.actionType,actionToTaken.actionDescription, actionToTaken.valueChange));
            transactionsBuffer.Add(newTransaction);

            Debug.Log($"Transaction created involving yourself and player {playerSelectionDropdown.value} to {actionToTaken.actionDescription} added to buffer");
        }
        else
        {
            Debug.LogWarning("Transaction Buffer full! Please send the block before create a new transaction");
        }
        
    }

    /// <summary>
    /// Creates a block out of transaction buffer and sends for validation
    /// </summary>
    public void SendBlock()
    {
        //Make sure we are actually wanting to create a block with transactions
        if(transactionsBuffer.Count <= 0)
        {
            Debug.LogWarning("Transaction Buffer Is empty! Try create a transaction first");
            return;
        }

        //Create new block from buffer data
        Block testBlock1 = new Block(blockchain[blockchain.Count - 1].GetCurrentBlockHash(), transactionsBuffer.ToArray());

        //Send a Net Message here containing the block data
        //Send block for validation
        Debug.Log("New block created...Getting read to send");
        networkManager.SendNetMessage(testBlock1, -1);

        //clear the buffer ready for next set of actions
        transactionsBuffer.Clear();
    }


    /// <summary>
    /// Debug the blockchain in console, Deprecated due to blockchain being visualized on screen
    /// </summary>
    public void onActionDebug()
    {
#pragma warning disable 0618
        DebugBlockchain();
    }
}
