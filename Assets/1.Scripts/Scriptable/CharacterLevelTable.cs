using System;
using UnityEngine;

public struct CharacterGrowthData
{
    public int Exp { get; set; } 
}

public static class CharacterLevelTable
{
    public static CharacterGrowthData GetCharacterGrowthData(int level)
    {
        CharacterGrowthData characterGrowthData = new CharacterGrowthData()
        {
            Exp = level * 10
        };
        return characterGrowthData;
    }
}
