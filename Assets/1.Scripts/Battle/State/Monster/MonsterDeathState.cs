using UnityEngine;

public class MonsterDeathState : CharacterState
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
