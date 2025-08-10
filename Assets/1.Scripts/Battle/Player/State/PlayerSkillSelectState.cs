using System;
using System.Collections;
using UnityEngine;

public class PlayerSkillSelectState : PlayerState
{
    public override void Execute()
    {
        var menu = SkillMenuSelectUI.Instance;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            character.SwapState("mainmenu");
            return;
        }
        if (menu.MenuSelected == false) return;
        
        var player = character as BattlePlayer;
        player.CurrentSelectSkillIndex = menu.CurrentSelectIndex;
        player.CurrentAttackType = BattleAttackType.Skill;
        character.SwapState("targetselect");
    }

    public override void Enter()
    {
        var menu = SkillMenuSelectUI.Instance;
        menu.gameObject.SetActive(true);
        menu.UpdateSelectUI(character as BattlePlayer);
        
        PlayerStatUI.Instance.gameObject.SetActive(true);
        PlayerStatUI.Instance.Setup(character as BattlePlayer);
    }

    public override void Exit()
    {
        var menu = SkillMenuSelectUI.Instance;
        menu.gameObject.SetActive(false);
        
        PlayerStatUI.Instance.gameObject.SetActive(false);

    }
}
