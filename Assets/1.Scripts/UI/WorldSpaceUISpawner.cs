using UnityEngine;

public class WorldSpaceUISpawner : MonoSingleton<WorldSpaceUISpawner>
{
    [SerializeField] private HpBarUI hpbarUIPrefab;
    [SerializeField] private StatusEffectUI statusEffectorUIPrefab;
    
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
}
