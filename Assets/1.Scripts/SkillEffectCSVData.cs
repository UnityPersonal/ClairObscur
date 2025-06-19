using System;
using UnityEngine;

public enum SkillEffectTargetGroup
{
    None = 0, // None
    Self = 1, // Self
    Enemy = 3 // Enemy
}

public enum SkillTargetRangeType
{
    None = 0, // None
    Single = 1, // Single
    All = 2 // All
}

public enum SkillStatusEffectType
{
    None = 0, // None
    Damage = 1, // Damage
    Heal = 2, // Heal
    Fire = 3, // Fire
    Ice = 4, // Ice
    Stun = 5, // Stun
    Reverse = 6, // Reverse
    Debuff = 4 // Debuff
}

public enum CharacterClass
{
    None = 0, // None
    Warrior = 1, // Warrior
    Archer = 2, // Archer
    Goblin = 3, // Goblin
    Ghost = 4, // Ghost
    Boss = 5 // Boss
}

[Serializable]
public class SkillEffectCSVData
{
    public string SkillName { get; set; } // 스킬 이름
    public string ActionName { get; set; } // 스킬이 적용되는 액션 이름
    public int APCost { get; set; } // 스킬 사용 시 소모되는 AP (Action Point)
    public int LearningCost { get; set; } // 스킬 학습 비용
    public int CharacterClass { get; set; }  // 0: None, 1: Warrior, 2: Archer, 3: Goblin, 4: Ghost, 5: Boss
    public int EffectTargetGroup { get; set; } // 0: None, 1: Self, 2: Ally, 3: Enemy
    public int EffectTargetRange { get; set; }  // 0: None, 1: Single, 2: All
    public int EffectType { get; set; }        // 0: None, 1: Damage, 2: Heal, 3: Fire, 4: Debuff
    public int EffectValue { get; set; }       // 효과의 수치 (예: 데미지(%), 힐량(%), 버프/디버프 지속시간(turn(integer)) 등)
}
