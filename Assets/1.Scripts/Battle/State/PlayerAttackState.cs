using System.Collections;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public override IEnumerator Execute(BattlePlayer player)
    {
        yield return StartCoroutine(player.AttackCoroutine());
    }
}
