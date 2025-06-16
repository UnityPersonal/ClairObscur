using System;
using System.Collections;
using UnityEngine;

public class PlayerSkillSelectState : PlayerState
{
    public override IEnumerator Execute(BattlePlayer player)
    {
        var menu = SkillMenuSelectUI.Instance;
        menu.gameObject.SetActive(true);
        yield return StartCoroutine(menu.UpdateSelectUI(player));
        var selectType = menu.CurrentSelectType;
        switch (selectType)
        {
            case SkillMenuSelectUI.SelectType.Skill1:
                player.CurrentAttackType = BattleAttackType.Skill1;
                player.NextAction = BattlePlayer.ActionType.TargetSelect;
                break;
            case SkillMenuSelectUI.SelectType.Skill2:
                player.CurrentAttackType = BattleAttackType.Skill2;
                player.NextAction = BattlePlayer.ActionType.TargetSelect;
                break;
            case SkillMenuSelectUI.SelectType.Skill3:
                player.CurrentAttackType = BattleAttackType.Skill3;
                player.NextAction = BattlePlayer.ActionType.TargetSelect;
                break;
            case SkillMenuSelectUI.SelectType.MainMenu:
                player.NextAction = BattlePlayer.ActionType.MainMenu;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        menu.gameObject.SetActive(false);
    }
}
