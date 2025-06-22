using System;
using UnityEngine;


public enum SkillEffectGroup
{
    Team,
    Enemy,
    All,
}
public enum SkillEffectRange
{
    SingleTarget, // 단일 대상
    AllTargets, // 전체 대상
    Self, // 자신
}

public class SkillEffectorHandler
{
    public string EffectorName { get; set; }
    public string EffectorCondition { get; set; } // 조건에 대한 설명
    public SkillEffectGroup EffectorGroup { get; set; } // 적용 대상 그룹
    public SkillEffectRange EffectorRange { get; set; } // 적용 범위
    public int EffectorValue { get; set; } // 효과 값
    
    private int previousValue = 0;

    public SkillEffectorHandler(){}

    
    public SkillEffectorHandler(
        string effectorName,
        string effectorCondition,
        SkillEffectGroup effectorGroup,
        SkillEffectRange effectorRange,
        int effectorValue)
    {
        this.EffectorName = effectorName;
        this.EffectorCondition = effectorCondition;
        this.EffectorGroup = effectorGroup;
        this.EffectorRange = effectorRange;
        this.EffectorValue = effectorValue;
    }

    public void ApplySkillEffect()
    {
        var turnCharacter = BattleManager.Instance.CurrentTurnCharacter;
        switch (EffectorGroup)
        {
            case SkillEffectGroup.Team:
                ApplySkillEffectToGroup(turnCharacter.CharacterLayer);
                break;
            case SkillEffectGroup.Enemy:
                ApplySkillEffectToGroup(turnCharacter.EnemyLayer);
                break;
            case SkillEffectGroup.All:
                ApplySkillEffectToGroup(turnCharacter.CharacterLayer);
                ApplySkillEffectToGroup(turnCharacter.EnemyLayer);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ApplySkillEffectToGroup(BattleCharacterLayer layer)
    {
        
        switch (EffectorRange)
        {
            case SkillEffectRange.SingleTarget:
                var currentTurnCharacter = BattleManager.Instance.CurrentTurnCharacter;
                ApplySkillEffectToTarget(currentTurnCharacter.Target);
                break;
            case SkillEffectRange.AllTargets:
                var group = BattleManager.Instance.AlivedCharacterGroup(layer);
                foreach (var character in group) ApplySkillEffectToTarget(character);
                break;
            case SkillEffectRange.Self:
                var selfCharacter = BattleManager.Instance.CurrentTurnCharacter;
                ApplySkillEffectToTarget(selfCharacter);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    

    private void ApplySkillEffectToTarget(BattleCharacter character)
    {
        if (character == null)
        {
            Debug.LogError("SkillEffectorHandler ::: ApplySkillEffectToTarget - Character is null");
            return;
        }

        var effector = character.StatusEffect(EffectorName);
        previousValue = effector.EffectorValue;
        effector.EffectorValue = EffectorValue;
    }
    
    public void RemoveSkillEffect()
    {
        var turnCharacter = BattleManager.Instance.CurrentTurnCharacter;
        switch (EffectorGroup)
        {
            case SkillEffectGroup.Team:
                RemoveSkillEffectToGroup(turnCharacter.CharacterLayer);
                break;
            case SkillEffectGroup.Enemy:
                RemoveSkillEffectToGroup(turnCharacter.EnemyLayer);
                break;
            case SkillEffectGroup.All:
                RemoveSkillEffectToGroup(turnCharacter.CharacterLayer);
                RemoveSkillEffectToGroup(turnCharacter.EnemyLayer);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void RemoveSkillEffectToGroup(BattleCharacterLayer layer)
    {
        
        switch (EffectorRange)
        {
            case SkillEffectRange.SingleTarget:
                var currentTurnCharacter = BattleManager.Instance.CurrentTurnCharacter;
                RemoveSkillEffectToTarget(currentTurnCharacter.Target);
                break;
            case SkillEffectRange.AllTargets:
                var group = BattleManager.Instance.AlivedCharacterGroup(layer);
                foreach (var character in group) RemoveSkillEffectToTarget(character);
                break;
            case SkillEffectRange.Self:
                var selfCharacter = BattleManager.Instance.CurrentTurnCharacter;
                RemoveSkillEffectToTarget(selfCharacter);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void RemoveSkillEffectToTarget(BattleCharacter character)
    {
        if (character == null)
        {
            Debug.LogError("SkillEffectorHandler ::: ApplySkillEffectToTarget - Character is null");
            return;
        }

        var effector = character.StatusEffect(EffectorName);
        effector.EffectorValue = previousValue;
    }
}