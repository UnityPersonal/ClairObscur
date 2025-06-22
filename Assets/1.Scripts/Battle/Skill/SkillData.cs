using UnityEngine;
using UnityEngine.Timeline;

public class SkillData
{
    public string SkillName;
    public string SkillDescription; // 스킬 설명
    public Sprite SkillIcon; // 스킬 아이콘
    
    public BattleActionController action;
    
    public int ApCost; // 스킬 배우는 비용
    public int LearnCost; // 스킬 사용 비용

    public SkillEffectorHandler dealEffectHandler;
    public SkillEffectorHandler buffEffectHandler;
    
    public SkillData defendEffectHandler;

    public SkillData(SkillCSVData skillCSVData)
    {
        this.SkillName = skillCSVData.SkillName;
        this.SkillDescription = skillCSVData.Description;
        this.ApCost = skillCSVData.APCost;
        this.LearnCost = skillCSVData.LearningCost;
        this.SkillIcon = Resources.Load<Sprite>(skillCSVData.SkillIconPath);
        this.action = Resources.Load<BattleActionController>(skillCSVData.ActionPath);

        this.dealEffectHandler = new SkillEffectorHandler();
        this.buffEffectHandler = new SkillEffectorHandler();
    }
    
}

[System.Serializable]
public class SkillCSVData
{
    public string SkillName { get; set; }
    public int APCost { get; set; }
    public int LearningCost { get; set; }
    public string PreLearnSkill { get; set; }

    public string ActionPath { get; set; } // Path to the action associated with the skill
    public string SkillIconPath { get; set; } // Path to the skill icon
    public string Description { get; set; } // Description of the skill

    public string DealEffector { get; set; } // Effect that deals damage
    public string DealRange { get; set; } // Range of the effect that deals damage
    public int DealValue { get; set; } // Value of the effect that deals damage
    
    public string BuffEffector { get; set; } // Effect that applies a buff
    public string BuffGroup { get; set; } // Group of the buff effect
    public string BuffRange { get; set; } // Range of the buff effect
    public int BuffValue { get; set; } // Value of the buff effect
    
}
