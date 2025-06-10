using System;
using System.Collections;
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
                    yield return StartCoroutine(MainMenuCoroutine()) ;
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
        
    }

    private IEnumerator MainMenuCoroutine()
    {
        while (IsDead == false)
        {
            yield return null;
        }
    }

    private IEnumerator TargetSelectCoroutine()
    {
        while (IsDead == false)
        {
            yield return null;
        }
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
        while (IsDead == false)
        {
            yield return null;
        }
        Activated = false;
    }

    
}
