using System;
using UnityEngine;
using UnityEngine.Timeline;

public class SkillData
{
    public string Key;
    
    public string SkillName;
    public string SkillDescription; // 스킬 설명
    public Sprite SkillIcon; // 스킬 아이콘
    
    
    public BattleActionController action;
    
    public int ApCost; // 스킬 배우는 비용
    public int LearnCost; // 스킬 사용 비용
    public string preLearnSkill; // 선행 스킬 키, 이 스킬을 배우기 전에 반드시 배워야 하는 스킬의 키

    public DealEffectorHandler dealEffectHandler;
    public SkillEffectorHandler buffEffectHandler;
    SkillEffectGroup ToGroup(string group)
    {
        group = group.ToLower();
        return group switch
        {
            "_" => SkillEffectGroup.Team,
            "enemy" => SkillEffectGroup.Enemy,
            "team" => SkillEffectGroup.Team,
            "all" => SkillEffectGroup.All,
            _ => SkillEffectGroup.Team,
            
        };
    }
        
    SkillEffectRange ToRange(string range)
    {
        range = range.ToLower();
        return range switch
        {
            "_" => SkillEffectRange.Self,
            "self" => SkillEffectRange.Self,
            "single" => SkillEffectRange.SingleTarget,
            "all" => SkillEffectRange.AllTargets,
            _ => SkillEffectRange.Self,
        };
    }
    
    public SkillData(SkillCSVData skillCSVData, SkillDatabase skillDatabase)
    {
        Key = skillCSVData.Key;
        SkillName = skillCSVData.SkillName;
        SkillDescription = skillCSVData.Description;
        ApCost = skillCSVData.APCost;
        LearnCost = skillCSVData.LearningCost;
        SkillIcon = skillDatabase.iconTable[skillCSVData.IconPath];
        action = skillDatabase.actionTable.GetActionData(skillCSVData.ActionPath);

        preLearnSkill = skillCSVData.PreLearnSkill;
        
        
        dealEffectHandler = new DealEffectorHandler();
        dealEffectHandler.EffectorName = skillCSVData.DealEffector;
        dealEffectHandler.EffectorRange = ToRange(skillCSVData.DealRange);
        dealEffectHandler.EffectorValue = skillCSVData.DealValue;
        
        buffEffectHandler = new SkillEffectorHandler();
        buffEffectHandler.EffectorName = skillCSVData.BuffEffector;
        buffEffectHandler.EffectorGroup = ToGroup(skillCSVData.BuffEffector);
        buffEffectHandler.EffectorRange = ToRange(skillCSVData.BuffRange);
        buffEffectHandler.EffectorValue = skillCSVData.BuffValue;
        
        /*Debug.Log($"{SkillName}" +
                  $" AP Cost: {ApCost}," +
                  $" Learn Cost: {LearnCost}" +
                  $" Description: {SkillDescription}" +
                  $" Action Path: {skillCSVData.ActionPath}" +
                  $" Skill Icon Path: {skillCSVData.IconPath}" +
                  $"PreLearnSkill: {preLearnSkill}" );*/

    }
    
}

[System.Serializable]
public class SkillCSVData
{
    public string Key { get; set; } // Unique key for the skill, e.g., "skill_001"
    public string SkillName { get; set; }
    public int APCost { get; set; }
    public int LearningCost { get; set; }
    public string PreLearnSkill { get; set; }

    public string ActionPath { get; set; } // Path to the action associated with the skill
    public string IconPath { get; set; } // Path to the skill icon
    public string Description { get; set; } // Description of the skill

    public string DealEffector { get; set; } // Effect that deals damage
    public string DealRange { get; set; } // Range of the effect that deals damage
    public int DealValue { get; set; } // Value of the effect that deals damage
    
    public string BuffEffector { get; set; } // Effect that applies a buff
    public string BuffGroup { get; set; } // Group of the buff effect
    public string BuffRange { get; set; } // Range of the buff effect
    public int BuffValue { get; set; } // Value of the buff effect
    
}
