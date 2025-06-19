using System;
using UnityEngine;

/// <summary>
/// 캐릭터가 스킬을 사용할 수 없게 합니다.
/// </summary>
public class SilenceEffector : StatusEffector
{
    // Skill 시스템과의 연동이 필요합니다.
    protected override void BindToEvents()
    {
        // 예: SkillSystem.OnTryCast += BlockSkill;
    }
}