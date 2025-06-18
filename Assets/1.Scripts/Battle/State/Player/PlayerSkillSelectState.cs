using System;
using System.Collections;
using UnityEngine;

public class PlayerSkillSelectState : PlayerState
{
    public override void Execute()
    {
        var menu = SkillMenuSelectUI.Instance;
        if (menu.MenuSelected == false) return;
        
        var selectType = menu.CurrentSelectType;
        switch (selectType)
        {
            case SkillMenuSelectUI.SelectType.Skill1:
                character.CurrentAttackType = BattleAttackType.Skill1;
                character.SwapState("targetslect");
                break;
            case SkillMenuSelectUI.SelectType.Skill2:
                character.CurrentAttackType = BattleAttackType.Skill2;
                character.SwapState("targetslect");
                break;
            case SkillMenuSelectUI.SelectType.Skill3:
                character.CurrentAttackType = BattleAttackType.Skill3;
                character.SwapState("targetslect");
                break;
            case SkillMenuSelectUI.SelectType.MainMenu:
                character.SwapState("mainmenu");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override void Enter()
    {
        var menu = SkillMenuSelectUI.Instance;
        menu.gameObject.SetActive(true);
        menu.UpdateSelectUI(character as BattlePlayer);
    }

    public override void Exit()
    {
        var menu = SkillMenuSelectUI.Instance;
        menu.gameObject.SetActive(false);
    }
}
