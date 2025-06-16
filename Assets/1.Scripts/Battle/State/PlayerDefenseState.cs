using System;
using System.Collections;
using UnityEngine;

public class PlayerDefenseState : PlayerState
{
    public enum DefenseType
    {
        None,
        Parry,
        ParryAttack,
        Dodge,
        Jump,
        JumpAttack,
    }
    public DefenseType defenseType = DefenseType.None;
    
    public override IEnumerator Execute(BattlePlayer player)
    {
        switch (defenseType)
        {
            case DefenseType.None:
                yield break;
            case DefenseType.Parry:
                yield return ParryingCoroutine(player);
                break;
            case DefenseType.ParryAttack:
                yield return ParryingAttackCoroutine(player);
                break;
            case DefenseType.Dodge:
                yield return DodgeCoroutine(player);
                break;
            case DefenseType.Jump:
                yield return JumpingCoroutine(player);
                yield break;
            case DefenseType.JumpAttack:
                yield break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    protected IEnumerator DodgeCoroutine(BattlePlayer player)
    {
        var actionData =  player.ActionLUT.GetActionData(ActionDataType.Dodge);
        var timeline = actionData.actionTimeline;
        player.DodgeActionTime = Time.time; // 가장 최근 회피 시간을 캐싱한다.
        Debug.Log($"<color=red>{gameObject.name} Dodge Play </color>");
        yield return player.PlayTimeline(timeline,player.CharacterDefaultLocation, player.DodgeTransform);
    }

    private IEnumerator ParryingCoroutine(BattlePlayer player)
    {
        var actionData =  player.ActionLUT.GetActionData(ActionDataType.Parry);
        var timeline = actionData.actionTimeline;
        player.ParryActionTime = Time.time;
        Debug.Log($"<color=blue>{gameObject.name} Parry Play </color>");
        yield return player.PlayTimeline(timeline, player.CharacterDefaultLocation, player.CharacterDefaultLocation);
    }

    private IEnumerator ParryingAttackCoroutine(BattlePlayer player)
    {
        var actionData =  player.ActionLUT.GetActionData(ActionDataType.ParryingAttack);
        var timeline = actionData.actionTimeline;
        Debug.Log($"<color=yellow>{gameObject.name} Parry Attack Play </color>");
        yield return player.PlayTimeline(timeline, player.CharacterDefaultLocation, player.CharacterDefaultLocation);
    }
    
    private IEnumerator JumpingCoroutine(BattlePlayer player)
    {
        player.JumpActionTime = Time.time; // 가장 최근 점프 시간을 캐싱한다.
        yield break;
    }
}
