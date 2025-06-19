using System;
using UnityEngine;

/// <summary>
/// 캐릭터가 회복을 받을 수 없게 합니다.
/// </summary>
public class BindEffector : StatusEffector
{
    // Heal 차단은 외부 회복 시스템과의 연동이 필요합니다.
    protected override void BindToEvents()
    {
        // 예: HealManager.OnBeforeHeal += OnBeforeHeal;
    }
}