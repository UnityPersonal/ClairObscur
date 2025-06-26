using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattlePlayerInfoUI : MonoBehaviour
{
    [SerializeField] TMP_Text playerNameText; // Text component to display the player's name
    [SerializeField] Slider hpSlider; // Text component to display the player's hp slider
    [SerializeField] TMP_Text hpText; // Text component to display the player's HP text
    [SerializeField] TMP_Text apText; // Text component to display the player's AP text
    [SerializeField] RectTransform apBar; // RectTransform for the AP bar
    [SerializeField] Image systemImage; // Image component for the system image

    BattlePlayer player;
    
    public void OnStatusChanged()
    {
        PlayerStatus status = player.playerStatus;
        // Update the player's name
        playerNameText.text = status.CharacterName;

        // Update HP slider and text
        var hp = status.GetStat(GameStat.HEALTH);
        hpSlider.maxValue = hp.MaxValue;
        hpSlider.value = hp.StatValue;
        hpText.text = $"{hp.StatValue}/{hp.MaxValue}"; // Display current HP and max HP

        // Update AP text
        var ap = status.GetStat(GameStat.AP);
        apText.text = ap.StatValue.ToString();

        for(int i = 0 ; i < ap.MaxValue; i++)
        {
            if (i < ap.StatValue)
            {
                apBar.GetChild(i).gameObject.SetActive(true); // Show AP bar segments based on current AP
            }
            else
            {
                apBar.GetChild(i).gameObject.SetActive(false); // Hide AP bar segments beyond current AP
            }
        }
    }
    
    public void Setup(BattlePlayer player)
    {
        this.player = player;
        player.Callbacks.OnStatusChanged += OnStatusChanged; // Subscribe to status changes
        OnStatusChanged();
    }
}
