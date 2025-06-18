using System;
using System.Collections;
using UnityEngine;

public class PlayerMainMenuState : PlayerState
{
    public override void Execute()
    {
        var menu = MainMenulSelectUI.Instance;
        if (menu.MenuSelected == false)
            return;
        
        var selectType = menu.CurrentSelectType;
        switch (selectType)
        {
            case MainMenulSelectUI.SelectType.Attack:
                character.CurrentAttackType = BattleAttackType.Normal;
                character.SwapState("targetslect");
                break;
            case MainMenulSelectUI.SelectType.Skill:
                character.SwapState("skillselect");
                break;
            case MainMenulSelectUI.SelectType.Item:
                character.SwapState("itemselect");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override void Enter()
    {
        var menu = MainMenulSelectUI.Instance;
        menu.gameObject.SetActive(true);
        menu.UpdateSelectUI(character as BattlePlayer );
    }

    public override void Exit()
    {
        var menu = MainMenulSelectUI.Instance;
        menu.gameObject.SetActive(false);
    }
}
