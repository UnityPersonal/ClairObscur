using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

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

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnAttack(AttackEventArgs args)
    {
        if (args.Target.Equals(this) == true)
        {
            Debug.Log($"{args.Attacker.name} attacked {name} for {args.Damage} damage.");
            animator.SetTrigger("Hit");
            BattleEventManager.OnTakeDamage(
                new TakeDamageEventArgs(this, args.Damage)
            );
        }
    }

    
    ActionType NextAction = ActionType.MainMenu;

    private BattleCharacter targetCharacter = null;

    public override void OnEmittedBeginAttackSignal()
    {
        BattleEventManager.OnAttack( new AttackEventArgs(10, this, targetCharacter));
    }

    public override void OnEmittedBeginDefendSignal()
    {
        // begin defend 시 처리
        // activate?
        Activated = true;
    }

    public override void OnEmittedEndDefendSignal()
    {
        Activated = false;
    }

    public override BattleCharacterType CharacterType => BattleCharacterType.Player;
    public override BattleCharacter TargetCharacter => targetCharacter;

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
        List<BattleCharacter> alivedEnemyList = new List<BattleCharacter>();        
        foreach (var character in enemyList)
        {
            if (character.IsDead == false)
            {
                alivedEnemyList.Add(character);
            }
        }
        // 살아있는 적중에 랜덤으로 타겟을 지정한다.
        targetCharacter = alivedEnemyList[Random.Range(0, alivedEnemyList.Count)];
        
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
    
    
    

    
}
