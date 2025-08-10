using UnityEngine;
using UnityEngine.Playables;

public class MonsterAttackState : CharacterState
{
    public override void Execute()
    {
    }

    public override void Enter()
    {
        var monster = character as BattleMonster;
        var playerList = BattleManager.Instance.CharacterGroup[BattleCharacterLayer.Player];
        foreach (var character in playerList)
        {
            if(character.IsDead == false)
            {
                monster.playerTargetCharacter = character;
                break;
            }
        }
        monster.SwapAction("attack",(PlayableDirector _) => 
        {
            monster.SwapState("wait");
        });
    }

    public override void Exit()
    {
    }
}
