using UnityEngine;

public class MonsterWaitState : CharacterState
{
    public override void Execute()
    {
    }

    public override void Enter()
    {
        character.SwapAction("wait");
        character.Deactivate();
    }

    public override void Exit()
    {
    }
}
