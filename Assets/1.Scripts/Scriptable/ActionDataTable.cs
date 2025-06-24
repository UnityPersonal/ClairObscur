using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionDataTable", menuName = "Scriptable Objects/ActionDataTable")]
public class ActionDataTable : ScriptableObject
{
    public BattleActionController[] actionDataList;
    
    public BattleActionController GetActionData(string actionDataType)
    {
        foreach (var actionData in actionDataList)
        {
            if (actionData.ActionName.Equals(actionDataType, StringComparison.OrdinalIgnoreCase))
            {
                return actionData;
            }
        }
        return null; 
    }
}
