using UnityEngine;

public class PlayerDeathState : PlayerState
{

    public override void Execute()
    {
    }

    public override void Enter()
    {
        character.SwapAction("death");
    }

    public override void Exit()
    {
    }
}
