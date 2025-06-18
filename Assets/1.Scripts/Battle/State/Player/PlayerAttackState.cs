using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerAttackState : PlayerState
{
    public override void Execute()
    {
    }
    
    public override void Enter()
    {
        character.SwapAction("attack", (PlayableDirector _) =>
        {
            character.SwapState("wait");
        });
        
    }

    public override void Exit()
    {
    }
}
