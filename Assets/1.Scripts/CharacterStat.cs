using System;
using UnityEngine;

[Serializable]
public class CharacterStat
{
    [SerializeField] private int maxHP;
    public int MaxHP => maxHP;
    
    private int currentHP;
    public int CurrentHP
    {
        get => currentHP;
        set
        {
            currentHP = Mathf.Clamp(value, 0, maxHP);
        }
    }

    public int Level { get; private set; } = 1;
    public int CurrentExp { get; private set; } = 0;
    public int ExpToNextLevel => CharacterLevelTable.GetCharacterGrowthData(Level).Exp;
    
    public bool IsDead => currentHP <= 0;


}
