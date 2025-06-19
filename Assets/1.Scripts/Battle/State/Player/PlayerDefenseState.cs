using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;

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

    private bool isActing = false;
    
    public override void Execute()
    {
        var player = character as BattlePlayer;
        if (isActing == true) return;

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
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            player.JumpActionTime = Time.time;
            isActing = true;
            character.SwapAction("jump", (PlayableDirector _) => { OnEndDefenseAction();});
        }
    }

    private void OnEndDefenseAction()
    {
        isActing = false;
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
