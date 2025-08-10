using UnityEngine;
using UnityEngine.Playables;

public class PlayerCounterState : PlayerState
{
    public override void Execute()
    {
    }

    public override void Enter()
    {
        character.SwapAction("counter", (PlayableDirector _) =>
        {
            character.SwapState("wait");
        });
    }

    public override void Exit()
    {
    }
}