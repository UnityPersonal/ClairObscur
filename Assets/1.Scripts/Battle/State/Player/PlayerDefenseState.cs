using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerDefenseState : PlayerState
{
    public enum DefenseType
    {
        None,
        Parry,
        ParryAttack,
        Dodge,
    }
    public DefenseType defenseType = DefenseType.None;

    private bool isActing = false;
    
    bool reserveSwap = false;
    
    public override void ReserveSwap(string nextState)
    {
        base.ReserveSwap(nextState);
        // Do not allow swapping to another state while acting
        if (isActing) reserveSwap = true;
        else character.SwapState(nextState,true);
    }
    
    public override void Execute()
    {
        var player = character as BattlePlayer;
        if (isActing == true) return;
        
        if(reserveSwap == true) 
        {
            reserveSwap = false;
            character.SwapState("wait", true);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            player.DodgeActionTime = Time.time;
            isActing = true;
            character.SwapAction("dodge", (PlayableDirector _) => { OnEndDefenseAction(); });
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            player.ParryActionTime = Time.time;
            isActing = true;
            character.SwapAction("parry", (PlayableDirector _) => { OnEndDefenseAction();});
        }
       
    }

    private void OnEndDefenseAction()
    {
        isActing = false;
        reserveSwap = false;
        if (character.IsDead == false)
        {
            character.SwapAction("wait");
        }
    }

    public override void Enter()
    {
        isActing = false;
    }

    public override void Exit()
    {
    }
}
