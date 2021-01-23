using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class User : BlockchainManager
{
    private NET_NetworkManager networkManager;

    [SerializeField]
    private Dropdown playerSelectionDropdown;

    private List<Transaction> transactionsBuffer = new List<Transaction>();

    [Range(1,5)]
    [SerializeField]
    private int imaxBufferSize = 3;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = FindObjectOfType<NET_NetworkManager>();
    }

    //Used to determine which action to create and add to buffer
    public void onActionClickButton(ActionEventPass actionToTaken)
    {

        if (transactionsBuffer.Count < imaxBufferSize)
        {
            Transaction newTransaction = new Transaction(networkManager.myID, playerSelectionDropdown.value - 1, new Action(actionToTaken.actionType,actionToTaken.actionDescription, actionToTaken.valueChange));
            transactionsBuffer.Add(newTransaction);

            Debug.LogError($"Transaction created involving yourself and player {playerSelectionDropdown.value} to {actionToTaken.actionDescription} added to buffer");
        }
        else
        {
            Debug.LogError("Transaction Buffer full! Please send the block before create a new transaction");
        }
        
    }

    //Creates a block out of transaction buffer and sends for validation
    public void SendBlock()
    {
        //Make sure we are actually wanting to create a block with transactions
        if(transactionsBuffer.Count <= 0)
        {
            Debug.LogError("Transaction Buffer Is empty! Try create a transaction first");
            return;
        }

        //Create new block from buffer data
        Block testBlock1 = new Block(blockchain[blockchain.Count - 1].GetCurrentBlockHash(), transactionsBuffer.ToArray());

        //Send a Net Message here containing the block data
        //Send block for validation
        Debug.LogError("New block created...Getting read to send");
        networkManager.SendNetMessage(testBlock1, -1);

        //clear the buffer ready for next set of actions
        transactionsBuffer.Clear();
    }    


    //Debug the blockchain in console, Deprecated due to blockchain being visualized on screen
    public void onActionDebug()
    {
        DebugBlockchain();
    }
}
