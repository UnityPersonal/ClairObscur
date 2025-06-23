using System;
using System.Collections.Generic;
using UnityEngine;



public class StatusEffectUI : WorldSpaceUIFollow
{
    [SerializeField] StatusEffectTileUI tileUIPrefab;
    [SerializeField] RectTransform statusEffectContainer;

    private Dictionary<string, StatusEffectTileUI> tileUIDictionary = new Dictionary<string, StatusEffectTileUI>();
    
    public void SetUp(BattleCharacter character)
    {
        this.character = character;
        var assetList = AssetManager.Instance.CommonEffectorList;
        foreach (var asset in assetList)
        {
            var key = asset.EffectorName.ToLower();
            tileUIDictionary[key] = Instantiate(tileUIPrefab, statusEffectContainer);
            tileUIDictionary[key].SetUp(asset.EffectorIcon, 0); 
        }

        OnChanagedCharacterStatusEffects();
        character.Callbacks.OnEffectorChanged += OnChanagedCharacterStatusEffects;
    }
    
    private void OnChanagedCharacterStatusEffects()
    {
        foreach (var pair in character.StatusEffects)
        {
            var effector = pair.Value;
            var key = effector.EffectorName.ToLower();
            if (tileUIDictionary.TryGetValue(key, out var tileUI))
            {
                if (effector.EffectorValue > 0)
                {
                    tileUI.gameObject.SetActive(true);
                    tileUI.SetUp(effector.EffectorIcon, effector.EffectorValue);
                }
                else
                {
                    tileUI.gameObject.SetActive(false);
                }
            }
        }
    }
}
