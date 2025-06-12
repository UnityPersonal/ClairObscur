using UnityEngine;

[System.Serializable]
public class BattleCharacterSpawnData
{
    public string spawnName;
    public BattleCharacter character;
}

[CreateAssetMenu(fileName = "BattleCharacterSpawnTable", menuName = "Scriptable Objects/BattleCharacterSpawnTable")]
public class BattleCharacterSpawnTable : ScriptableObject
{
    public BattleCharacterSpawnData[] spawnDatas;
    
    public BattleCharacter GetCharacterSample(string spawnName)
    {
        spawnName = spawnName.ToLower();
        foreach (var spawnData in spawnDatas)
        {
            if (spawnData.spawnName == spawnName)
            {
                return spawnData.character;
            }
        }
        Debug.LogWarning($"Character with name {spawnName} not found in BattleCharacterSpawnTable.");
        return null;
    }
}


