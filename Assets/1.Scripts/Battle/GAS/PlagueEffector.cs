using System;
using UnityEngine;

/// <summary>
/// 캐릭터의 최대 생명력을 감소시킵니다.
/// 다수의 역병에 감염될 수도 있으며, 각각의 역병이 최대 생명력을 10% 감소시킵니다.
/// </summary>
public class PlagueEffector : StatusEffector
{
    protected override void BindToEvents()
    {
        BattleEventManager.Callbacks.OnTakeDamage += OnTakeDamage;
    }

    protected override void OnTakeDamage(TakeDamageEventArgs args)
    {
        if (args.Target == owner)
        {
            int reduce = Mathf.CeilToInt(owner.Status.MaxHP * 0.1f);
            Debug.Log($"[PlagueEffector] {owner.name} 최대 HP 감소 {reduce}");
            owner.Status.CurrentHP = Mathf.Min(owner.Status.CurrentHP, owner.Status.MaxHP - reduce);
        }
    }
}
