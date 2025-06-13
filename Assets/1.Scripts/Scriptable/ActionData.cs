using UnityEngine;
using UnityEngine.Timeline;

public enum ActionDataType
{
    Attack,
    Dodge,
    Parry,
    JumpAttack,
    JumpDodge,
    Shoot,
    Skill
}

[CreateAssetMenu(fileName = "ActionData", menuName = "Scriptable Objects/ActionData")]
public class ActionData : ScriptableObject
{
    public ActionDataType actionDataType;
    public TimelineAsset actionTimeline;
}
