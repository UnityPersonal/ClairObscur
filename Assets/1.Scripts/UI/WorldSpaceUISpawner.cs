using System;
using System.Linq;
using UnityEngine;

public class WorldSpaceUISpawner : MonoSingleton<WorldSpaceUISpawner> , IWorldUI
{
    [SerializeField] private HpBarUI hpbarUIPrefab;
    [SerializeField] private StatusEffectUI statusEffectorUIPrefab;
    
    public RectTransform RectTransform => transform as RectTransform;
    public void SpawnHpBar(BattleCharacter character)
    {
        if (character == null)
        {
            Debug.LogError("BattleCharacter is null. Cannot spawn HP bar.");
            return;
        }

        var hpBarUI = Instantiate(hpbarUIPrefab, transform);
        hpBarUI.name = $"{hpBarUI.name} {character.CharacterName}";
        hpBarUI.SetUp(character);
    }
    
    public void SpawnStatusEffectUI(BattleCharacter character)
    {
        if (character == null)
        {
            Debug.LogError("BattleCharacter is null. Cannot spawn Status Effect UI.");
            return;
        }

        var statusEffectUI = Instantiate(statusEffectorUIPrefab, transform);
        statusEffectUI.name = $"{statusEffectUI.name} {character.CharacterName}";
        statusEffectUI.SetUp(character);
    }

    private void Update()
    {
        var worldUIs =GetComponentsInChildren<IWorldUI>( ).ToList();
        var camera = Camera.main;

        float CameraDistanceToUI(IWorldUI worldUi)
        {
            if (worldUi == null || worldUi.RectTransform == null)
                return float.MaxValue; // Handle null cases
            
            var uiPosition = worldUi.RectTransform.position;
            return Vector3.Distance(camera.transform.position, uiPosition);
        }
        worldUIs.Sort( (a, b) =>
        {
            if (a == null || b == null) return 0; // Handle null cases
            float distanceA = CameraDistanceToUI(a);
            float distanceB = CameraDistanceToUI(b);
            return distanceA < distanceB ? 1 : -1; // Sort by distance to camera
        });
        
        
        for (int i = 0; i < worldUIs.Count; i++)
        {
            if (worldUIs[i] == null || worldUIs[i].RectTransform == null)
                continue; // Skip null UI elements
            
            // Set sibling index based on distance to camera
            worldUIs[i].RectTransform.SetSiblingIndex(i);
        }
        
        Debug.Log($"WorldSpaceUISpawner Update: {worldUIs.Count} UIs sorted by distance to camera.");
        
    }
}
