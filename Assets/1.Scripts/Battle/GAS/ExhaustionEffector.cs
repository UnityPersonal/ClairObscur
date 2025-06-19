using System;
using UnityEngine;

/// <summary>
/// 캐릭터가 AP를 얻을 수 없게 합니다.
/// </summary>
public class ExhaustionEffector : StatusEffector
{
    // AP 획득 차단은 AP 시스템과의 연동이 필요합니다.
    protected override void BindToEvents()
    {
        // 예: APManager.OnGainAP += OnGainAP;
    }
}