using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class BlockchainDebugging : MonoBehaviour
{
    [SerializeField]
    private Transform contentHolder;

    [SerializeField]
    private GameObject blockUIPrefab;

    private List<GameObject> blocksUIList = new List<GameObject>();

    private List<Block> blockchainData = new List<Block>();
    private bool needsUIUpdating = true;

    //Update the UI representing blocks in the blockchain
    public void UpdateBlockchainData(List<Block> a_blockchainData)
    {
        blockchainData = a_blockchainData;
        needsUIUpdating = true;
    }

    
    private void Update()
    {
        //Due to UpdateBlockchainUI tracing back to a static method and we are wanting to create new object in scene
        //we would not be on the main thread, this is why we are checking for any updates in the blockchain data here
        //As we can then call update UI from here
        if(needsUIUpdating)
        {
            UpdateBlockchainUI();
        }
    }

    //Update the UI representing blocks in the blockchain
    public void UpdateBlockchainUI()
    {
        //destroy all current block to refresh list
        for (int i = 0; i < blocksUIList.Count; ++i)
        {
            Destroy(blocksUIList[i]);
        }

        //Clear the current list
        blocksUIList.Clear();

        Blockchain tempBlockchain = new Blockchain(blockchainData);
        int blockchainSize = tempBlockchain.GetBytes().Length;

        //Loop through every block in the blockchain data
        for (int i = 0; i < blockchainData.Count; ++i)
        {
            GameObject newBlock = Instantiate(blockUIPrefab, contentHolder);
            BlockUI blockUI = newBlock.GetComponent<BlockUI>();
            blockUI.SetupUI(Encoding.ASCII.GetString(blockchainData[i].GetCurrentBlockHash()),
                            Encoding.ASCII.GetString(blockchainData[i].GetPreviousBlockHash()),
                            blockchainData[i].GetTransactions().Length,
                            blockchainData[i].GetBytes().Length,
                            blockchainSize);

            blocksUIList.Add(newBlock);
        }

        //As we have now updating the UI, make sure we dont call again
        needsUIUpdating = false;
    }

}
