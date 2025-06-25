using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public interface ILoadableAsset
{
    void LoadAsset();
}

[Serializable]
public class CharacterAssetTable
{
    public string tableKey = "CharacterAssetTable";
    public StatusEffectorAssetTable characterEffectorTable;
    public SkillDatabase skillDatabase;
    public CharacterLevelTable characterLevelTable;
    public ActionDataTable actionDataTable;
    public IconDatabase iconDatabase;
}

public class AssetManager : DontDestorySingleton<AssetManager>
{
    [SerializeField] private StatusEffectorAssetTable commonEffectorTable;
    public StatusEffectorAssetTable dealEffectorTable;
    public StatusEffectorAssetTable buffEffectorTable;
    
    public List<CharacterAssetTable> characterAssetTables = new List<CharacterAssetTable>();

    public CharacterAssetTable GetCharacterAssetTable(string tableKey)
    {
        return characterAssetTables.FirstOrDefault(table => table.tableKey.Equals(tableKey, StringComparison.OrdinalIgnoreCase));
    }
    public List<StatusEffectorAsset> CommonEffectorList => commonEffectorTable.AssetList;
    public StatusEffectorAsset GetStatusEffectorAsset(string effectorName)    
    {
        if (commonEffectorTable == null)
        {
            Debug.LogError("StatusEffectorAssetTable is not assigned in AssetManager.");
            return null;
        }
        
        var effector = commonEffectorTable.GetStatusEffector(effectorName);
        if (effector == null)
        {
            Debug.LogWarning($"StatusEffector '{effectorName}' not found in the asset table.");
        }
        
        return effector;
    }
}
