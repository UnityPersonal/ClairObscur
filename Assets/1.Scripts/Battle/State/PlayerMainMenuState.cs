using System;
using System.Collections;
using UnityEngine;

public class PlayerMainMenuState : PlayerState
{
    public override IEnumerator Execute(BattlePlayer player)
    {
        Debug.Log("Main Menu Open");
        var menu = MainMenulSelectUI.Instance;
        menu.gameObject.SetActive(true);
        yield return StartCoroutine(menu.UpdateSelectUI(player));
        
        var selectType = menu.CurrentSelectType;
        switch (selectType)
        {
            case MainMenulSelectUI.SelectType.Attack:
                player.CurrentAttackType = BattleAttackType.Normal;
                player.NextAction = BattlePlayer.ActionType.TargetSelect;
                break;
            case MainMenulSelectUI.SelectType.Skill:
                player.NextAction = BattlePlayer.ActionType.SkillSelect;
                break;
            case MainMenulSelectUI.SelectType.Item:
                player.NextAction = BattlePlayer.ActionType.MainMenu;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        menu.gameObject.SetActive(false);

    }
    
}
