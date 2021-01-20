using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

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

    public byte[] GetBytes()
    {
        //Calculate total size of the byte array
        int stringBytesCount = ASCIIEncoding.Unicode.GetByteCount(actionTaken);
        int totalBytesCount = (sizeof(int) * 2) + stringBytesCount;

        byte[] combinedData = new byte[totalBytesCount];

        byte[] currentUserIDBytes = BitConverter.GetBytes(currentUserID);
        currentUserIDBytes.CopyTo(combinedData, 0);

        byte[] targetUserIDBytes = BitConverter.GetBytes(currentUserID);
        targetUserIDBytes.CopyTo(combinedData, 4);

        byte[] actionTakenBytes = Encoding.ASCII.GetBytes(actionTaken);
        targetUserIDBytes.CopyTo(combinedData, 8);

        return combinedData;

    }
}
