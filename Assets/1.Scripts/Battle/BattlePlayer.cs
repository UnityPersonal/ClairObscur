using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BattlePlayer : BattleCharacter
{
    enum ActionType
    {
        MainMenu,
        Attack,
        SkillSelect,
        TargetSelect,
        SkillActive,
    }

    private void OnEnable()
    {
        var callbacks = BattleEventManager.Callbacks;
        callbacks.OnAttack += OnAttack;
    }

    private void OnAttack(AttackEventArgs args)
    {
        if (args.Target.Equals(this) == true)
        {
            Debug.Log($"{args.Attacker.name} attacked {name} for {args.Damage} damage.");
        }
    }

    
    ActionType NextAction = ActionType.MainMenu;

    private BattleCharacter targetCharacter = null;

    protected override IEnumerator UpdateBattleActionCoroutine()
    {
        NextAction =  ActionType.MainMenu; 
        
        while (Activated && !IsDead)
        {
            switch (NextAction)
            {
                case ActionType.MainMenu:
                    yield return StartCoroutine(MainMenuCoroutine());
                    break;
                case ActionType.Attack:
                    yield return StartCoroutine(AttackCoroutine());
                    break;
                case ActionType.SkillSelect:
                    yield return StartCoroutine(SkillSelectCoroutine());
                    break;
                case ActionType.TargetSelect:
                    yield return StartCoroutine(TargetSelectCoroutine());
                    break;
                case ActionType.SkillActive:
                    yield return StartCoroutine(SkillActiveCoroutine());
                    break;
            }
        }
        Debug.Log($"BattlePlayer Turn Ended");
    }

    private IEnumerator MainMenuCoroutine()
    {
        NextAction = ActionType.TargetSelect;
        yield break;
    }

    private IEnumerator TargetSelectCoroutine()
    {
        // 액션을 취할 대상을 선택합니다.
        // 상대 진영에서 적을 랜덤으로 선택합니다.
        var enemyList = BattleManager.Instance.CharacterGroup[BattleCharacterType.Enemy];
        foreach (var character in enemyList)
        {
            if (character.IsDead == false)
            {
                targetCharacter = character;
                break;
            }
        }
        
        Debug.Log($"BattlePlayer ::: TargetSelectCoroutine {name} selected target {targetCharacter.name}.");
        NextAction = ActionType.Attack;
        yield break;
    }

    private IEnumerator SkillSelectCoroutine()
    {
        while (IsDead == false)
        {
            yield return null;
        }
    }

    
    private IEnumerator SkillActiveCoroutine()
    {
        while (IsDead == false)
        {
            yield return null;
        }
        Activated = false;
    }
    
    private IEnumerator AttackCoroutine()
    {
        // 대상하게 공격을 수행합니다.
        Debug.Log($"BattlePlayer ::: AttackCoroutine {name} is attacking {targetCharacter.name}.");
        BattleEventManager.OnAttack( new AttackEventArgs(10, this, targetCharacter));
        while (Input.GetKeyDown(KeyCode.Space) == false)
        {
            yield return null;
        }
        Activated = false;
        
        yield break;
    }

    
}
