using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Transaction
{
    private int currentUserID = -1;
    private int targetUserID = -1;
    private string actionTaken = "";

    public Transaction(int a_currentUserID, int a_targetUserID, string a_actionTaken)
    {
        currentUserID = a_currentUserID;
        targetUserID = a_targetUserID;
        actionTaken = a_actionTaken;
    }

    public int getCurrentUserID()
    {
        return currentUserID;
    }

    public int getTargetUserID()
    {
        return targetUserID;
    }

    public string getActionTaken()
    {
        return actionTaken;
    }
}
