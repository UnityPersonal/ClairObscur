using UnityEngine;

public class MonsterActiveState : CharacterState
{
    private float waitTime = 0.0f;
    public override void Execute()
    {
        waitTime += Time.deltaTime;
        if (waitTime >= 2f)
        {
            character.SwapState("attack");
        }
        
    }

    public override void Enter()
    {
        waitTime = 0.0f;
    }

    public override void Exit()
    {
    }
}