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
    
    [SerializeField] StatusEffectTileUI tileUIPrefab;
    [SerializeField] RectTransform statusEffectContainer;
    
    public void SetUp(BattleCharacter character)
    {
        var childCount = statusEffectContainer.childCount;
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(statusEffectContainer.GetChild(i).gameObject);
        }

        foreach (var statusEffector in character.StatusEffects)
        {
            var tileUI = Instantiate(tileUIPrefab, statusEffectContainer);
            tileUI.SetUp(statusEffector.Value.EffectorIcon, statusEffector.Value.EffectorValue);
        }
    }
}
