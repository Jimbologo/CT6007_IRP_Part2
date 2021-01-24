using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines what type of action was taken
/// </summary>
[System.Serializable]
public enum ActionType
{
    TakeCoins = 0,
    GiveCoins,
    TakeHealth,
    GiveHealth
}

/// <summary>
/// These are actions taken by clients ready to be added to a transaction
/// </summary>
[System.Serializable]
public class Action
{
    public ActionType actionType;
    public string actionDescription;
    public int valueChange;

    /// <summary>
    /// Constructure
    /// </summary>
    /// <param name="a_actionType"></param>
    /// <param name="a_actionDescription"></param>
    /// <param name="a_valueChange"></param>
    public Action(ActionType a_actionType, string a_actionDescription, int a_valueChange)
    {
        actionType = a_actionType;
        actionDescription = a_actionDescription;
        valueChange = a_valueChange;
    }

}
