using System;
using UnityEngine;

/// <summary>
/// 지속시간이 끝나면 캐릭터를 처치합니다.
/// </summary>
public class CurseEffector : StatusEffector
{
    private float remainingTurns = 3;
    public override string EffectorName => "Curse";

    protected override void BindToEvents()
    {
        BattleEventManager.Callbacks.OnStartTurn += OnStartTurn;
    }

    protected override void OnStartTurn(StartTurnEventArgs args)
    {
        if (args.Character != owner) return;

        remainingTurns -= 1;
        if (remainingTurns <= 0)
        {
            Debug.Log($"[CurseEffector] {owner.name}가 저주로 사망합니다.");
            BattleEventManager.OnDeath(new DeathEventArgs(owner));
        }
    }
}