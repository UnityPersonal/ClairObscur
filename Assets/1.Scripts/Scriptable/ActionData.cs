using UnityEngine;
using UnityEngine.Timeline;


[CreateAssetMenu(fileName = "ActionData", menuName = "Scriptable Objects/ActionData")]
public class ActionData : ScriptableObject
{
    public string actionDataType;
    public BattleActionController controller;
}
