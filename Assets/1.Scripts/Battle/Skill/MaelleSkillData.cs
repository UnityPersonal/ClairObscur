public class MaelleSkillData : SkillData
{
    public MaelleSkillData(SkillCSVData skillCSVData,SkillDatabase skillDatabase) : base(skillCSVData,skillDatabase)
    {
    }
}

[System.Serializable]
public class MaelleSkillCSVData : SkillCSVData
{
    public string StenceEffector { get; set; } // Effect that upgrades the skill
}