using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;

    public int additiveDamage; // 추가 데미지 (예: 버프 등으로 인한)
    public int multiplierDamage; // 배수 데미지 (예: 특정 스킬의 효과로 인한)
    
    public int CalcaultedDamage (int damage)    
    {
        return (damage + additiveDamage) * multiplierDamage;
    }
    
    // 스킬 배우는 비용
    public string LearnCost;
    // 스킬 사용 비용
    public int ApCost;

    public BattleActionController action;
}
