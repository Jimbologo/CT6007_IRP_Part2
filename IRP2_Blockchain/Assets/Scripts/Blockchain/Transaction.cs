using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Holds all action data as well as users involved in a transaction for a block 
/// </summary>
[System.Serializable]
public class Transaction
{
    private int currentUserID = -1;
    private int targetUserID = -1;
    private Action actionTaken;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="a_currentUserID"></param>
    /// <param name="a_targetUserID"></param>
    /// <param name="a_actionTaken"></param>
    public Transaction(int a_currentUserID, int a_targetUserID, Action a_actionTaken)
    {
        currentUserID = a_currentUserID;
        targetUserID = a_targetUserID;
        actionTaken = a_actionTaken;
    }

    /// <summary>
    /// Gets current User ID
    /// </summary>
    /// <returns></returns>
    public int getCurrentUserID()
    {
        return currentUserID;
    }

    /// <summary>
    /// Gets Target User ID
    /// </summary>
    /// <returns></returns>
    public int getTargetUserID()
    {
        return targetUserID;
    }

    /// <summary>
    /// Gets the action that was taken
    /// </summary>
    /// <returns></returns>
    public Action getActionTaken()
    {
        return actionTaken;
    }

    /// <summary>
    /// Converts the objetc to bytes and returns
    /// </summary>
    /// <returns></returns>
    public byte[] GetBytes()
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, this);
            return ms.ToArray();
        }

    }
}
