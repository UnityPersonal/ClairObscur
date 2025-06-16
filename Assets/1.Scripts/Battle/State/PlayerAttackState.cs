using System.Collections;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public override BattlePlayer.ActionType StateType => BattlePlayer.ActionType.Attack;

    public override IEnumerator Execute(BattlePlayer player)
    {
        yield return StartCoroutine(player.AttackCoroutine());
    }
}
