using UnityEngine;

public class MonsterDeathState : CharacterState
{
    public override void Execute()
    {
    }

    public override void Enter()
    {
        character.SwapAction("death");
        character.Deactivate();
    }

    public override void Exit()
    {
    }
}
