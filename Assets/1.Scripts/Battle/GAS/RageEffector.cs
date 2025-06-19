using System;
using UnityEngine;

/// <summary>
/// 캐릭터가 2번 연속으로 플레이할 수 있게 합니다.
/// </summary>
public class RageEffector : StatusEffector
{
    protected override void BindToEvents()
    {
        BattleEventManager.Callbacks.OnStartTurn += OnStartTurn;
    }

    protected override void OnStartTurn(StartTurnEventArgs args)
    {
        if (args.Character == owner)
        {
            Debug.Log($"[RageEffector] {owner.name}가 2번 행동합니다.");
            // BattleTurnManager.QueueExtraTurn(owner); // 구현 필요
        }
    }
}