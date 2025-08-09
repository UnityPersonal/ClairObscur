using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[System.Serializable]
public class PlayerStatus : CharacterStatus
{
    [TableList]
    [SerializeField] public List<BattleAttribute> Attributes = new List<BattleAttribute>();
    public BattleAttribute GetAttribute(string name)
    {
        return Attributes.Find(attr => attr.AttributeName.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
    
    public override void BindCharacter(BattleCharacter owner)
    {
        base.BindCharacter(owner);
        foreach (var attr in Attributes)
        {
            attr.BindCharacter(owner);
        }
        
    }
    
    const int MaxEquipSkillCount = 3;
    
    public SkillData[] EquippedSkills = new SkillData[3];
    public List<SkillData> LearnedSkills = new List<SkillData>();
}