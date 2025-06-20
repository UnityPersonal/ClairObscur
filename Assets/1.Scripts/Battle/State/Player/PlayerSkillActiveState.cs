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
        
        // todo: 스킬 효과 적용
        character.SwapAction(action.ActionName, (PlayableDirector _) =>
        {
            character.SwapState("wait");
        });
        
    }

    public override void Exit()
    {
    }
}
