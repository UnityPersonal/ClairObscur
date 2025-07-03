using System;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerStatMenu : MonoBehaviour
{
    [SerializeField] private PlayerStatLineUI level;
    [SerializeField] private PlayerStatLineUI health;
    [SerializeField] private PlayerStatLineUI power;
    [SerializeField] private PlayerStatLineUI defense;
    [SerializeField] private PlayerStatLineUI critical;
    
    [SerializeField] private TMP_Text attributePointText;
    
    [SerializeField] private PlayerStatLineUI vitality;
    [SerializeField] private PlayerStatLineUI might;
    [SerializeField] private PlayerStatLineUI defenseAttr;
    [SerializeField] private PlayerStatLineUI luck;
        
    
    private PlayerStatus playerStatus;

    private void Start()
    {
        vitality.incrementButton.onClick.AddListener(() => IncrementAttribute("vitality"));
        vitality.decrementButton.onClick.AddListener(() => DecrementAttribute("vitality"));
        
        might.incrementButton.onClick.AddListener(() => IncrementAttribute("might"));
        might.decrementButton.onClick.AddListener(() => DecrementAttribute("might"));
        
        defenseAttr.incrementButton.onClick.AddListener(() => IncrementAttribute("defense"));
        defenseAttr.decrementButton.onClick.AddListener(() => DecrementAttribute("defense"));
        
        luck.incrementButton.onClick.AddListener(() => IncrementAttribute("luck"));
        luck.decrementButton.onClick.AddListener(() => DecrementAttribute("luck"));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void BindPlayer(string playerName)
    {
        playerStatus = GameUser.Instance.GetPlayerStatus(playerName);
        GameUser.Instance.UpdateStatus();
        UpdateUI();
    }

    void UpdateUI()
    {
        var levelStat = playerStatus.GetStat(CharacterStatus.LEVEL);
        level.statValue.text = levelStat.StatValue.ToString();
        var healthStat = playerStatus.GetStat(CharacterStatus.HEALTH);
        health.statValue.text = $"{healthStat.MaxValue}";
        var powerStat = playerStatus.GetStat(CharacterStatus.ATTACK_POWER);
        power.statValue.text = powerStat.StatValue.ToString();
        var defenseStat = playerStatus.GetStat(CharacterStatus.DEFENSE);
        defense.statValue.text = defenseStat.StatValue.ToString();
        var criticalStat = playerStatus.GetStat(CharacterStatus.CRITICAL_RATE);
        critical.statValue.text = criticalStat.StatValue.ToString();
        var attributePointStat = playerStatus.GetStat(CharacterStatus.ATTRIBUTE_POINT);
        attributePointText.text = attributePointStat.StatValue.ToString();
        
        var vitalityAttr = playerStatus.GetAttribute("vitality");
        vitality.statValue.text = vitalityAttr.AttributeValue.ToString();
        vitality.incrementButton.gameObject.SetActive(attributePointStat.StatValue > 0);
        vitality.decrementButton.gameObject.SetActive(vitalityAttr.AttributeValue != 0);
        
        var mightAttr = playerStatus.GetAttribute("might");
        might.statValue.text = mightAttr.AttributeValue.ToString();
        might.incrementButton.gameObject.SetActive(attributePointStat.StatValue > 0);
        might.decrementButton.gameObject.SetActive(mightAttr.AttributeValue != 0);
        
        var defenseAttribute = playerStatus.GetAttribute("defense");
        defenseAttr.statValue.text = defenseAttribute.AttributeValue.ToString();
        defenseAttr.incrementButton.gameObject.SetActive(attributePointStat.StatValue > 0);
        defenseAttr.decrementButton.gameObject.SetActive(defenseAttribute.AttributeValue != 0);
        
        var luckAttr = playerStatus.GetAttribute("luck");
        luck.statValue.text = luckAttr.AttributeValue.ToString();
        luck.incrementButton.gameObject.SetActive(attributePointStat.StatValue > 0);
        luck.decrementButton.gameObject.SetActive(luckAttr.AttributeValue != 0);
    }
    
    public void IncrementAttribute(string attributeName)
    {
        // Implement logic to increment the specified attribute
        // For example, increase the character's strength, agility, etc.
        Debug.Log($"Incrementing attribute: {attributeName}");
        var attribute = playerStatus.GetAttribute(attributeName);
        var attributePointStat = playerStatus.GetStat(CharacterStatus.ATTRIBUTE_POINT);
        if (attribute != null)
        {
            attribute.IncrementAttributeValue(1); // Increment the attribute value by 1
            attributePointStat.IncrementStatValue(-1); // Decrement the attribute point by 1
            GameUser.Instance.UpdateStatus();
            UpdateUI(); // Refresh the UI after incrementing
        }
        else
        {
            Debug.LogWarning($"Attribute '{attributeName}' not found.");
        }
    }
    
    public void DecrementAttribute(string attributeName)
    {
        // Implement logic to decrement the specified attribute
        // For example, decrease the character's strength, agility, etc.
        Debug.Log($"Decrementing attribute: {attributeName}");
        var attribute = playerStatus.GetAttribute(attributeName);
        var attributePointStat = playerStatus.GetStat(CharacterStatus.ATTRIBUTE_POINT);
        if (attribute != null)
        {
            attribute.DecrementAttributeValue(1); // Decrement the attribute value by 1
            attributePointStat.IncrementStatValue(1); // Increment the attribute point by 1
            GameUser.Instance.UpdateStatus();
            UpdateUI(); // Refresh the UI after decrementing
        }
        else
        {
            Debug.LogWarning($"Attribute '{attributeName}' not found.");
        }
    }
}
