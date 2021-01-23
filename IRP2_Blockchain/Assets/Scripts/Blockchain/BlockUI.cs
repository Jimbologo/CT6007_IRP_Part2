using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class BlockUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentHashTextUI;

    [SerializeField]
    private TextMeshProUGUI previousHashTextUI;

    [SerializeField]
    private TextMeshProUGUI transactionCountTextUI;

    [SerializeField]
    private TextMeshProUGUI blockSizeTextUI;

    [SerializeField]
    private TextMeshProUGUI blockchainSizeTextUI;

    //Update the Text UI elements with data
    public void SetupUI(string currentHash, string previousHash, int transactionCount, int blockSizeInBytes, int blockchainSizeInBytes)
    {
        currentHashTextUI.text = "This Hash: " + currentHash;
        previousHashTextUI.text = "Prev Hash: " + previousHash;
        transactionCountTextUI.text = "NO. Transactions: " + transactionCount.ToString();
        blockSizeTextUI.text = "Size: " + blockSizeInBytes + "bytes";
        blockchainSizeTextUI.text = "BC Size: " + blockchainSizeInBytes + "bytes";
    }
}
