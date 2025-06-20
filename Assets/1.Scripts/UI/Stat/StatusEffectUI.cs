using System;
using System.Collections.Generic;
using UnityEngine;



public class StatusEffectUI : MonoBehaviour
{
    [Serializable]
    public class EffectIcon
    {
        public StatusEffectorType key;
        public Sprite icon;
    }
    
    public List<EffectIcon> effectIcons = new List<EffectIcon>();
    
    [SerializeField] RectTransform statusEffectContainer;
    
    public void SetUp(BattleCharacter character)
    {
        
    }
}
