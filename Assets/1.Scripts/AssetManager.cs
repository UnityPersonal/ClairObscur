using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssetManager : MonoSingleton<AssetManager>
{
    [SerializeField] private StatusEffectorAssetTable statusEffectorAssetTable;
    public List<StatusEffectorAsset> StatusEffectorAssetList => statusEffectorAssetTable.AssetList;
    public StatusEffectorAsset GetStatusEffectorAsset(string effectorName)    
    {
        if (statusEffectorAssetTable == null)
        {
            Debug.LogError("StatusEffectorAssetTable is not assigned in AssetManager.");
            return null;
        }
        
        var effector = statusEffectorAssetTable.GetStatusEffector(effectorName);
        if (effector == null)
        {
            Debug.LogWarning($"StatusEffector '{effectorName}' not found in the asset table.");
        }
        
        return effector;
    }
}
