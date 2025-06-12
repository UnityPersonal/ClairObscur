using UnityEngine;

[CreateAssetMenu(fileName = "ActionDataTable", menuName = "Scriptable Objects/ActionDataTable")]
public class ActionDataTable : ScriptableObject
{
    public ActionData[] actionDataList;
    
    public ActionData GetActionData(ActionDataType actionDataType)
    {
        foreach (var actionData in actionDataList)
        {
            if (actionData.actionDataType == actionDataType)
            {
                return actionData;
            }
        }
        return null; // or throw an exception if not found
    }
}
