using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterStatus
{
    public string CharacterName;
    public List<GameStat> stats = new List<GameStat>();
    public int CurrentHP
    {
        get => GetStat(GameStat.HEALTH).StatValue;
        set
        {
            var stat = GetStat(GameStat.HEALTH);
                stat.SetStatValue(value);
        }
    }
    
    public bool IsDead => CurrentHP <= 0;
    
    public GameStat GetStat(string statName)
    {
        var stat = stats.Find( (stat) => stat.StatName.Equals(statName, StringComparison.OrdinalIgnoreCase));
        if(stat == null)
        {
            Debug.LogWarning($"Stat '{statName}' not found in character stats.");
            return null;
        }
        return stat;
    }

    public List<bool> StatusEffects { get; private set; } = new List<bool>();

    public CharacterStatus()
    {
        StatusEffects = new List<bool>(GameUtilHelper.GetEnumCount<SkillStatusEffectType>() );
    }

}
