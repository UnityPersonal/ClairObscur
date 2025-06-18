using UnityEngine;

[CreateAssetMenu(fileName = "SkillDatabase", menuName = "Scriptable Objects/SkillDatabase")]
public class SkillDatabase : ScriptableObject
{
    [SerializeField] SkillData[] skills;
    
    public SkillData GetSkillData(string skillName)
    {
        foreach (var skill in skills)
        {
            if (skill.skillName == skillName)
            {
                return skill;
            }
        }
        return null; // or throw an exception if not found
    }
}
