using UnityEngine;
using UnityEngine.Timeline;

public enum ActionDataType
{
    Wait,
    Active,
    Attack,
    Dodge,
    Parry,
    ParryingAttack,
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
    public BattleActionController controller;
}
