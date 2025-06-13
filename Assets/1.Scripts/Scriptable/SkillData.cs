using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;

    public int damage; // 콤보 공격시 각 피격당 데미지 계산 필요
    
    // 스킬 배우는 비용
    public string LearnCost;
    // 스킬 사용 비용
    public int ApCost;
    public TimelineAsset timeline;
}
