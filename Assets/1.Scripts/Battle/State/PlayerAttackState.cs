using System.Collections;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public override IEnumerator Execute(BattlePlayer player)
    {
        player.SwapAction(ActionDataType.Attack);
        yield return StartCoroutine(player.AttackCoroutine());
    }
}
