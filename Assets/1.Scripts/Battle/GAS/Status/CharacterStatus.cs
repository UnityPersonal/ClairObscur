using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public partial class CharacterStatus
{
    public string CharacterName;
    [TableList()]
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
    
    public virtual void BindCharacter(BattleCharacter owner)
    {
        foreach (var stat in stats)
        {
            stat.BindCharacter(owner);
        }
    }
    
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
