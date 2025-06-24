using System;
using UnityEngine;

[Serializable]
public partial class GameStat
{
    BattleCharacter owner;
    public void BindCharacter(BattleCharacter character)
    {
        owner = character;
    }
    
    public string StatName = "Unnamed Stat";
    public string StatDescription = "No description provided.";
    [SerializeField] private int statValue = 0;
    [SerializeField] private int maxValue = -1; // -1 means no limit


    public int StatValue
    {
        get
        {
            return statValue;
        }
        set
        {
            if(maxValue < 0) statValue = value;
            else statValue = Math.Clamp(value, 0, maxValue);
            if (owner != null)
            {
                owner.Callbacks.OnStatusChanged?.Invoke();
            }
            else
            {
                Debug.LogWarning($"{StatName} Owner character is not bound to the GameStat .");
            }
        }
    }
    
    public int MaxValue
    {
        get
        {
            return maxValue;
        }
        set
        {
            maxValue = value;
            if (owner != null)
            {
                owner.Callbacks.OnStatusChanged?.Invoke();
            }
            else
            {
                Debug.LogWarning("Owner character is not bound to the GameStat.");
            }
        }
    }

    

    public override string ToString()
    {
        return $"{StatName}: {StatValue} / {MaxValue} - {StatDescription}";
    }
    
    public void IncrementStatValue(int increment)
    {
        SetStatValue(increment + statValue);
    }
    
    public void SetStatValue(int value)
    {
        if(MaxValue < 0) StatValue = value;
        else StatValue = Math.Clamp(value, 0, MaxValue);
    }
}