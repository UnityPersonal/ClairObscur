using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class AssetManager : DontDestorySingleton<AssetManager>
{
    [FormerlySerializedAs("statusEffectorAssetTable")] [SerializeField] private StatusEffectorAssetTable commonEffectorTable;
    public StatusEffectorAssetTable dealEffectorTable;
    public StatusEffectorAssetTable buffEffectorTable;
    public StatusEffectorAssetTable versoEffectorTable;
    public StatusEffectorAssetTable maelleEffectorTable;
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
