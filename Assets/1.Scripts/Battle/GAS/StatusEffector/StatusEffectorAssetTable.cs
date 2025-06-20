using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectorAssetTable", menuName = "GAS/StatusEffectorAssetTable", order = 1)]
public class StatusEffectorAssetTable : ScriptableObject
{
    [SerializeField] StatusEffectorAsset[] statusEffectors;
    public List<StatusEffectorAsset> AssetList => statusEffectors.ToList();

    public StatusEffectorAsset GetStatusEffector(string effectorName)
    {
        foreach (var effector in statusEffectors)
        {
            if (effector.EffectorName.Equals(effectorName, System.StringComparison.OrdinalIgnoreCase))
            {
                return effector;
            }
        }
        Debug.LogWarning($"StatusEffector '{effectorName}' not found in the asset table.");
        return null;
    }
    
}