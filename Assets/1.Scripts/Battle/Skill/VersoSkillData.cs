
using UnityEngine;

public class VersoSkillData : SkillData
{
    public int upgradeRank; // Rank of the skill upgrade
    public SkillEffectorHandler upgradeEffectHandler;
    
    public VersoSkillData(VersoSkillCSVData data, SkillDatabase skillDatabase)
    : base(data,skillDatabase)
    {
        int ToRank(string rank)
        {
            rank = rank.ToUpper();
            return rank switch
            {
                "D" => 0,
                "C" => 1,
                "B" => 2,
                "A" => 3,
                "S" => 4,
                _ => 0, // 기본값은 0
            };
        }
        upgradeRank = ToRank(data.UpgradeRank);
        
        upgradeEffectHandler = new SkillEffectorHandler();
        upgradeEffectHandler.EffectorName = data.UpgradeEffector;
        upgradeEffectHandler.EffectorGroup = SkillEffectGroup.Team;
        upgradeEffectHandler.EffectorRange = SkillEffectRange.Self;
        upgradeEffectHandler.EffectorValue = data.UpgradeValue;
    }
}

[System.Serializable]
public class VersoSkillCSVData : SkillCSVData
{
    public string UpgradeRank { get; set; } // Rank of the skill upgrade
    public string UpgradeEffector { get; set; } // Effect that upgrades the skill
    public int UpgradeValue { get; set; } // Value of the upgrade effect
    
}