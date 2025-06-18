using System.Collections;
using UnityEngine;

public class PlayerWaitState : PlayerState
{
    public override void Execute()
    {
    }

    public override void Enter()
    {
        character.Deactivate();
    }

    public override void Exit()
    {
    }
}
