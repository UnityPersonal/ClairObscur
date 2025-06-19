using UnityEngine;

[CreateAssetMenu(fileName = "ActionDataTable", menuName = "Scriptable Objects/ActionDataTable")]
public class ActionDataTable : ScriptableObject
{
    public BattleActionController[] actionDataList;
    
    public BattleActionController GetActionData(string actionDataType)
    {
        foreach (var actionData in actionDataList)
        {
            if (actionData.ActionName == actionDataType)
            {
                return actionData;
            }
        }
        return null; 
    }
}
