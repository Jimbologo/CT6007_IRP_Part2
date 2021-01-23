using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ActionEventPass : ScriptableObject
{
    public ActionType actionType;
    public string actionDescription;
    public int valueChange;

    public ActionEventPass(ActionType a_actionType, string a_actionDescription, int a_valueChange)
    {
        actionType = a_actionType;
        actionDescription = a_actionDescription;
        valueChange = a_valueChange;
    }
}
