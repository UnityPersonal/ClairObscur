using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    // 스킬 배우는 비용
    public string LearnCost;
    // 스킬 사용 비용
    public int ApCost;
    public TimelineAsset timeline;
}
