using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : BlockchainManager
{
    private NET_NetworkManager networkManager;
    // Start is called before the first frame update
    void Start()
    {
        networkManager = FindObjectOfType<NET_NetworkManager>();
    }


    public void onActionClickButton()
    {
        Transaction[] transactions1 = new Transaction[]
        {
            new Transaction(0,0,"abc"),
            new Transaction(0, 0, "abd"),
            new Transaction(0, 0, "abe"),
            new Transaction(0, 0, "abf"),
            new Transaction(0, 0, "abg"),
        };

        Block testBlock1 = new Block(blockchain[blockchain.Count - 1].GetCurrentBlockHash(), transactions1);

        //Send a Net Message here containing the block data
        //Send block for validation
        Debug.LogError("New block created...Getting read to send");
        networkManager.SendNetMessage(testBlock1, -1);
    }

    public void onActionClickButton2()
    {
        Transaction[] transactions1 = new Transaction[]
        {
            new Transaction(0,1,"ahc"),
            new Transaction(2, 2, "afd"),
            new Transaction(1, 0, "abe"),
            new Transaction(4, 3, "abf"),
            new Transaction(3, 4, "abg"),
        };

        Block testBlock1 = new Block(blockchain[blockchain.Count - 1].GetCurrentBlockHash(), transactions1);

        //Send a Net Message here containing the block data
        //Send block for validation
        networkManager.SendNetMessage(testBlock1, -1);
    }

    public void onActionDebug()
    {
        DebugBlockchain();
    }
}
