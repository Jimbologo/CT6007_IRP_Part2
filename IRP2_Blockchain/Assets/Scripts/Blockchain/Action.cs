using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ActionType
{
    TakeCoins = 0,
    GiveCoins,
    TakeHealth,
    GiveHealth
}

[System.Serializable]
public class Action
{
    public ActionType actionType;
    public string actionDescription;
    public int valueChange;

    public Action(ActionType a_actionType, string a_actionDescription, int a_valueChange)
    {
        actionType = a_actionType;
        actionDescription = a_actionDescription;
        valueChange = a_valueChange;
    }

}
