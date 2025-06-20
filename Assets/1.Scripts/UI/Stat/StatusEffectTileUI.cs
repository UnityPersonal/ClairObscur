using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StatusEffectTileUI : MonoBehaviour
{
    [SerializeField] private Image effectIconImage;
    [SerializeField] private TMP_Text countText;
    
    public void SetUp(Sprite icon, int count)
    {
        if (effectIconImage != null)
        {
            effectIconImage.sprite = icon;
            effectIconImage.enabled = icon != null;
        }
        
        if (countText != null)
        {
            countText.text = count > 1 ? count.ToString() : string.Empty;
            countText.enabled = count > 1;
        }
    }
}
