using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerActiveState : PlayerState
{
    public override void Execute()
    {
        character.SwapState("mainmenu");
    }

    public override void Enter()
    {
        character.SwapAction("active");
    }

    public override void Exit()
    {
    }
}
