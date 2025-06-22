
public class VersoSkillData : SkillData
{
    public SkillEffectorHandler upgradeEffectHandler;
    
    public VersoSkillData(VersoSkillCSVData data)
    : base(data)
    {
        SkillName = data.SkillName;
        SkillDescription = data.Description;
        ApCost = data.APCost;
        LearnCost = data.LearningCost;
        
        /*// Set the deal effect handler
        if (!string.IsNullOrEmpty(data.DealEffector))
        {
            dealEffectHandler = new SkillEffectorHandler(data.DealEffector, data.DealRange, data.DealValue);
        }
        
        // Set the buff effect handler
        if (!string.IsNullOrEmpty(data.BuffEffector))
        {
            buffEffectHandler = new SkillEffectorHandler(data.BuffEffector, data.BuffGroup, data.BuffRange, data.BuffValue);
        }
        
        // Set the upgrade effect handler
        if (!string.IsNullOrEmpty(data.UpgradeEffector))
        {
            upgradeEffectHandler = new SkillEffectorHandler(data.UpgradeEffector, data.UpgradeRank, data.UpgradeValue);
        }*/
        
        
    }
}

[System.Serializable]
public class VersoSkillCSVData : SkillCSVData
{
    public string UpgradeRank { get; set; } // Rank of the skill upgrade
    public string UpgradeEffector { get; set; } // Effect that upgrades the skill
    public float UpgradeValue { get; set; } // Value of the upgrade effect
    
}