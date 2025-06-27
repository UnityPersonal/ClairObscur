using System;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerSkillActiveState : PlayerState
{
    public override void Execute()
    {
    }
    
    void CastBuffEffect()
    {
        BattlePlayer player = character as BattlePlayer;
        var skillData = player.GetSkillDataByIndex(player.CurrentSelectSkillIndex);
    }

    public override void Enter()
    {
        BattlePlayer player = character as BattlePlayer;
        var skillData = player.GetSkillDataByIndex(player.CurrentSelectSkillIndex);
        
        if (skillData == null)
        {
            Debug.LogError("Skill data not found for index: " + player.CurrentSelectSkillIndex);
            return;
        }

        var action = skillData.action;
        // 코스트 소모
        var ap = player.Stat(GameStat.AP);
        ap.SetStatValue(ap.StatValue - skillData.ApCost);
        
        skillData.dealEffectHandler.ApplyDealEffect(character);
        skillData.buffEffectHandler.ApplySkillEffect();
        
        // todo: 스킬 효과 적용
        character.SwapAction(action.ActionName, (PlayableDirector _) =>
        {
            skillData.dealEffectHandler.RemoveDealEffect(character);
            skillData.buffEffectHandler.RemoveSkillEffect();
            character.SwapState("wait");
        });
        
    }

    public override void Exit()
    {
    }
}
