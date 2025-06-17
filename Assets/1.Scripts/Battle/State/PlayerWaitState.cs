using System.Collections;
using UnityEngine;

public class PlayerWaitState : PlayerState
{
    public override IEnumerator Execute(BattlePlayer player)
    {
        Debug.Log($"PlayerWaitState Execute {player.gameObject.name}");
        yield return player.WaitCoroutine();
    }
}
