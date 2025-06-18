using System;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerSkillActiveState : PlayerState
{
    public override void Execute()
    {
    }

    public override void Enter()
    {
        // todo: 스킬 활성화시 코스트 소모 및 스킬 추가효과 발동
        var player = character as BattlePlayer;
        switch (player.CurrentAttackType)
        {
            case BattleAttackType.Skill1:
                character.SwapAction("skill1", (PlayableDirector _) =>
                {
                    character.SwapState("wait");
                });
                break;
            case BattleAttackType.Skill2:
                character.SwapAction("skill2", (PlayableDirector _) =>
                {
                    character.SwapState("wait");
                });
                break;
            case BattleAttackType.Skill3:
                character.SwapAction("skill3", (PlayableDirector _) =>
                {
                    character.SwapState("wait");
                });
                break;
            case BattleAttackType.Normal:
            case BattleAttackType.Jump:
            case BattleAttackType.Gradient:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override void Exit()
    {
    }
}
