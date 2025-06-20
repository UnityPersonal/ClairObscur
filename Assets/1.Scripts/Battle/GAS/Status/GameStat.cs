using System;

[Serializable]
public partial class GameStat
{
    public string StatName = "Unnamed Stat";
    public string StatDescription = "No description provided.";
    public int StatValue = 0;
    public int MaxValue = 0;

    public override string ToString()
    {
        return $"{StatName}: {StatValue} / {MaxValue} - {StatDescription}";
    }
    
    public void SetStatValue(int value)
    {
        if(MaxValue < 0) StatValue = value;
        else StatValue = Math.Clamp(value, 0, MaxValue);
    }
        
}