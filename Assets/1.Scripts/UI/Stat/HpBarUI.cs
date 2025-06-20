using System;
using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : WorldSpaceUIFollow
{
    public Slider slider;


    public void SetUp(BattleCharacter character)
    {
        this.character = character;
        OnChangedStat();
        character.Callbacks.OnStatusChanged += OnChangedStat;
    }

    public void OnChangedStat()
    {
        var hp = character.Stat(GameStat.HEALTH);

        slider.value = hp.StatValue / (float)hp.MaxValue;

    }
}
