using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDataManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerText;
    [SerializeField]
    private TextMeshProUGUI coinsText;
    [SerializeField]
    private TextMeshProUGUI healthText;

    private int iplayerID = -1;
    private int icoins = 0;
    private int ihealth = 100;

    private int idefaultCoins = 0;
    private int idefaultHealth = 100;

    private bool bupdateRequired = false;

    private void Update()
    {
        if(bupdateRequired)
        {
            UpdateText();
        }
    }

    public void UpdateValues(int a_iplayerID)
    {
        iplayerID = a_iplayerID;
        bupdateRequired = true;
    }

    public void UpdateValues(int a_iplayerID, int a_icoins, int a_ihealth)
    {
        iplayerID = a_iplayerID;
        icoins = a_icoins;
        ihealth = a_ihealth;
        bupdateRequired = true;
    }

    //Update the text elements with the updated values
    private void UpdateText()
    {
        playerText.text = "Player: " + iplayerID;
        coinsText.text = "Coins: " + icoins;
        healthText.text = "Health: " + ihealth;

        bupdateRequired = false;
    }

    //We calculate our values from the blockchain
    public void CalculateBlockchain(List<Block> a_blockchain)
    {
        //We reset value to default
        icoins = idefaultCoins;
        ihealth = idefaultHealth;

        //we loop through each block and transaction, find action relating to us and apply
        for (int i = 0; i < a_blockchain.Count; ++i)
        {
            Transaction[] transactions = a_blockchain[i].GetTransactions();
            for (int j = 0; j < transactions.Length; ++j)
            {
                if (transactions[j] != null && transactions[j].getCurrentUserID() == iplayerID)
                {
                    //If i performed the action
                    Debug.LogError("I did something...");

                    //We now perform the action to update player data values
                    if(transactions[j].getActionTaken().actionType == ActionType.GiveCoins
                       || transactions[j].getActionTaken().actionType == ActionType.TakeCoins)
                    {
                        icoins -= transactions[j].getActionTaken().valueChange;
                    }
                }
                else if(transactions[j] != null && transactions[j].getTargetUserID() == iplayerID)
                {
                    //If i was the target of the action
                    Debug.LogError("someone did something to me...");

                    if (transactions[j].getActionTaken().actionType == ActionType.GiveCoins
                       || transactions[j].getActionTaken().actionType == ActionType.TakeCoins)
                    {
                        icoins += transactions[j].getActionTaken().valueChange;
                    }
                    else if (transactions[j].getActionTaken().actionType == ActionType.GiveHealth
                             || transactions[j].getActionTaken().actionType == ActionType.TakeHealth)
                    {
                        ihealth += transactions[j].getActionTaken().valueChange;
                    }
                }
            }
        }

        //Values have been updated so lets update the text components
        bupdateRequired = true;
    }
}
