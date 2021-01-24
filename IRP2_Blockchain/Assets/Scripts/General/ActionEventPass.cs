using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serilizable Actions to be able to pass action data to UserActionPanel
/// </summary>
[CreateAssetMenu]
public class ActionEventPass : ScriptableObject
{
    public ActionType actionType;
    public string actionDescription;
    public int valueChange;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="a_actionType"></param>
    /// <param name="a_actionDescription"></param>
    /// <param name="a_valueChange"></param>
    public ActionEventPass(ActionType a_actionType, string a_actionDescription, int a_valueChange)
    {
        actionType = a_actionType;
        actionDescription = a_actionDescription;
        valueChange = a_valueChange;
    }
}
