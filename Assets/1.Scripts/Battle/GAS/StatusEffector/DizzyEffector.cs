using System;
using UnityEngine;

/// <summary>
/// 캐릭터의 조준이나 조작을 어렵게 만듭니다.
/// </summary>
public class DizzyEffector : StatusEffector
{
    public override string EffectorName => "Vertigo";
    // UI 및 입력 시스템과의 연동이 필요합니다.
    protected override void BindToEvents()
    {
        // 예: InputManager.OnTargeting += ApplyDistortion;
    }
}