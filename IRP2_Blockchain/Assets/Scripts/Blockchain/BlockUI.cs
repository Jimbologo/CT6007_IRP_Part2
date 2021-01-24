using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Updates text elements for UI Block
/// </summary>
public class BlockUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentHashTextUI = null;

    [SerializeField]
    private TextMeshProUGUI previousHashTextUI = null;

    [SerializeField]
    private TextMeshProUGUI transactionCountTextUI = null;

    [SerializeField]
    private TextMeshProUGUI blockSizeTextUI = null;

    [SerializeField]
    private TextMeshProUGUI blockchainSizeTextUI = null;

    /// <summary>
    /// Update the Text UI elements with data
    /// </summary>
    /// <param name="currentHash"></param>
    /// <param name="previousHash"></param>
    /// <param name="transactionCount"></param>
    /// <param name="blockSizeInBytes"></param>
    /// <param name="blockchainSizeInBytes"></param>
    public void SetupUI(string currentHash, string previousHash, int transactionCount, int blockSizeInBytes, int blockchainSizeInBytes)
    {
        //Update String variables
        currentHashTextUI.text = "This Hash: " + currentHash;
        previousHashTextUI.text = "Prev Hash: " + previousHash;
        transactionCountTextUI.text = "NO. Transactions: " + transactionCount.ToString();
        blockSizeTextUI.text = "Size: " + blockSizeInBytes + "bytes";
        blockchainSizeTextUI.text = "BC Size: " + blockchainSizeInBytes + "bytes";
    }
}
