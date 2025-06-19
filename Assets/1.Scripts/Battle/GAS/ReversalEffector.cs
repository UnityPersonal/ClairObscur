using System;
using UnityEngine;

/// <summary>
/// 캐릭터가 치유를 받을 수 없게 합니다.
/// 반전 상태에서 치유되면 대신 피해를 받습니다.
/// </summary>
public class ReversalEffector : StatusEffector
{
    protected override void BindToEvents()
    {
        BattleEventManager.Callbacks.OnTakeDamage += OnTakeDamage;
    }

    protected override void OnTakeDamage(TakeDamageEventArgs args)
    {
        if (args.Target == owner && args.Damage < 0)
        {
            int reversed = Mathf.Abs(args.Damage);
            Debug.Log($"[ReversalEffector] {owner.name} 회복이 반전되어 {reversed} 피해");
            owner.Status.CurrentHP -= reversed;
        }
    }
}