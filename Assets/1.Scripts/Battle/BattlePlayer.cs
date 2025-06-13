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
    [SerializeField] Transform dodgeTransform;
    enum ActionType
    {
        None,
        MainMenu,
        Attack,
        SkillSelect,
        TargetSelect,
        SkillActive,
    }

    private bool isDefending = false;
    public bool IsDodging { get; private set; } = false;
    public bool IsParrying { get; private set; } = false;

    
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
            TakeDamage(args.Damage, args.Dodged, args.Parried, args.Jumped);
        }
    }

    protected override void OnDodge(DodgeEventArgs args)
    {
    }
    
    protected override void OnDeath(DeathEventArgs args)
    {
    }

    ActionType NextAction = ActionType.MainMenu;

    private BattleCharacter targetCharacter = null;

    public override void OnEmittedBeginAttackSignal()
    {
        int damage = 10;
        BattleEventManager.OnAttack( new AttackEventArgs
        (
            damage : damage,
            attacker: this,
            target: targetCharacter
        ));
    }

    public override void OnEmittedBeginDefendSignal()
    {
        Debug.Log("OnEmittedBeginDefendSignal");
        isDefending = true;
    }

    public override void OnEmittedEndDefendSignal()
    {
        Debug.Log("OnEmittedEndDefendSignal");
        isDefending = false;
    }

    public void OnEmittedBeginDodgeSignal()
    {
        IsDodging = true;
        animator.SetTrigger("Dodge");
        
    }
    
    public void OnEmittedEndDodgeSignal()
    {
        IsDodging = false;
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

    protected override IEnumerator UpdateDefendActionCoroutine()
    {
        while (gameObject.activeInHierarchy)
        {
            // dodge
            if (isDefending && Input.GetKeyDown(KeyCode.Q))
            {
                // 방어 중인 상태에서 처리할 로직을 여기에 작성합니다.
                // 예를 들어, 방어 애니메이션을 재생하거나 방어 상태를 표시하는 UI 업데이트 등을 할 수 있습니다.
                Debug.Log($"{name} is dodge.");
                yield return StartCoroutine(DodgeCoroutine());
                
            }
            // parry
            else if (isDefending && Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log($"{name} is parrying.");
                yield return StartCoroutine(ParryingCoroutine());
            }
            yield return null;
        }
    }
    
    protected IEnumerator DodgeCoroutine()
    {
        var actionData =  actionLUT.GetActionData(ActionDataType.Dodge);
        var timeline = actionData.actionTimeline;
        director.playableAsset = timeline;
        foreach (var track in timeline.GetOutputTracks())
        {
            if (track is AnimationTrack animationTrack)
            {
                Debug.Log($"AnimationTrack found: {animationTrack.name}");
                director.SetGenericBinding(animationTrack, animator);
            }
            
            // MoveToTargetTrack만 처리
            if (track is MoveToTargetTrack)
            {
                director.SetGenericBinding(track, transform);

                // 3. 트랙의 모든 클립 순회
                foreach (var clip in track.GetClips())
                {
                    // MoveToTargetClip만 처리
                    var moveClip = clip.asset as MoveToTargetClip;
                    if (moveClip != null)
                    {
                        // 4. actor, target 동적 할당
                        moveClip.actor.exposedName = UnityEditor.GUID.Generate().ToString();
                        moveClip.target.exposedName = UnityEditor.GUID.Generate().ToString();

                        if (clip.displayName.Equals("GoTo"))
                        {
                            director.SetReferenceValue(moveClip.actor.exposedName, characterDefaultLocation);
                            director.SetReferenceValue(moveClip.target.exposedName, dodgeTransform);
                        }
                        else if (clip.displayName.Equals("ReturnTo"))
                        {
                            director.SetReferenceValue(moveClip.actor.exposedName, dodgeTransform);
                            director.SetReferenceValue(moveClip.target.exposedName, characterDefaultLocation);
                        }
                        else
                        {
                            Debug.LogWarning($"clip displayName '{clip.displayName}' does not match expected names.");
                        }
                    }
                }
            }

            if (track is MoveToTargetTrack moveToTargetTrack)
            {
                director.SetGenericBinding(track, transform);
            }
        }
        
        DodgeEventArgs dodgeEventArgs = new DodgeEventArgs(Time.time);
        BattleEventManager.OnDodge(dodgeEventArgs);
        director.Play();
        Debug.Log($"<color=red>{gameObject.name} Dodge Play </color>");
        yield return WaitForTimeline(director);
    }

    private IEnumerator ParryingCoroutine()
    {
        yield break;
    }

    private IEnumerator MainMenuCoroutine()
    {
        NextAction = ActionType.TargetSelect;
        yield break;
    }

    private IEnumerator TargetSelectCoroutine()
    {
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
