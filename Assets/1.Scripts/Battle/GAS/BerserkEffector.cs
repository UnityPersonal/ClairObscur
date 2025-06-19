using System;
using UnityEngine;

/// <summary>
/// 매 턴마다 캐릭터의 피해를 증가시킵니다.
/// 최대 12번까지 누적될 수 있으며, 중첩 하나당 피해가 5% 증가합니다.
/// </summary>
public class BerserkEffector : StatusEffector
{
    private int stacks = 1;

    protected override void BindToEvents()
    {
        BattleEventManager.Callbacks.OnTakeDamage += OnTakeDamage;
    }

    protected override void OnTakeDamage(TakeDamageEventArgs args)
    {
        if (args.Target == owner)
        {
            stacks = Mathf.Min(stacks + 1, 12);
            int bonus = Mathf.CeilToInt(args.Damage * 0.05f * stacks);
            Debug.Log($"[BerserkEffector] {owner.name} 추가 피해 {bonus} (스택 {stacks})");
            owner.Status.CurrentHP -= bonus;
        }
    }
}