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
    
    private IEnumerator AttackCoroutine()
    {
        // 대상하게 공격을 수행합니다.
        Debug.Log($"BattlePlayer ::: AttackCoroutine {name} is attacking {targetCharacter.name}.");
        while (Input.GetKeyDown(KeyCode.Space) == false)
        {
            yield return null;
        }
        
        yield return StartCoroutine(PlayAttackAnimation());
        
        Activated = false;
        
        yield break;
    }
    
    IEnumerator PlayAttackAnimation()
    {
        // 1. TimelineAsset 가져오기
        var timeline = director.playableAsset as TimelineAsset;
        // 2. 모든 트랙 순회
        foreach (var track in timeline.GetOutputTracks())
        {
            // MoveToTargetTrack만 처리
            if (track is MoveToTargetTrack)
            {
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
                            director.SetReferenceValue(moveClip.target.exposedName, targetCharacter.CharacterHitTransform);
                        }
                        else if (clip.displayName.Equals("ReturnTo"))
                        {
                            director.SetReferenceValue(moveClip.actor.exposedName, targetCharacter.CharacterHitTransform);
                            director.SetReferenceValue(moveClip.target.exposedName, characterDefaultLocation);
                        }
                        else
                        {
                            Debug.LogWarning($"clip displayName '{clip.displayName}' does not match expected names.");
                        }
                        
                    }
                    
                }
            }

            if (track is BattleSignalEmitTrack)
            {
                director.SetGenericBinding(track, TimelineEventListener.Instance);
            }
        }

        director.Play();
        yield return WaitForTimeline(director);

        Debug.Log("타임라인 종료됨, 다음 단계 진행");
    }


    public static IEnumerator WaitForTimeline(PlayableDirector director)
    {
        bool isDone = false;

        void OnStopped(PlayableDirector _) => isDone = true;

        director.stopped += OnStopped;

        // 이미 재생 중인 경우만 대기
        if (director.state == PlayState.Playing)
            yield return new WaitUntil(() => isDone);

        director.stopped -= OnStopped;
    }

    
}
