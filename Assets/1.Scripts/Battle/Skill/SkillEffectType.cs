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

