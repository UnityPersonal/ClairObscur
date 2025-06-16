using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public enum BattleCharacterType
{
    Player,
    Enemy
}

public enum BattleAttackType
{
    Normal,
    Skill1,
    Skill2,
    Skill3,
    Jump,
    Gradient,
}

public abstract class BattleCharacter : MonoBehaviour
{
    const float AttackDelay = 0.33f; // Delay before the monster can attack again
    const float ParryDelay = 0.16f; // Delay before the monster can attack again
    
    [Header("Character Settings")]
    [SerializeField] protected string characterName;
    public string CharacterName => characterName;
    
    [Header("Character Location Settings")]
    [SerializeField] protected Transform characterDefaultLocation;
    [SerializeField] protected Transform characterHitTransform;
    public Transform CharacterHitTransform { get { return characterHitTransform; } }
    
    [Space(10), Header("Character Camera Settings")]
    [SerializeField] private CinemachineCamera characterCamera;
    
    [Space(10), Header("Character Timeline Settings")]
    [SerializeField] protected PlayableDirector director;
    
    [Space(10), Header("Character Animation Settings")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected ActionDataTable actionLUT;

    abstract public void OnBeginAttackSignal();
    abstract public void OnBeginDefendSignal();
    abstract public void OnEndDefendSignal();
    abstract public void OnCheckParriedSignal();

    abstract public  BattleCharacterType CharacterType { get; }
    abstract public BattleCharacter TargetCharacter { get; }
    
    public BattleAttackType CurrentAttackType { get; set; }= BattleAttackType.Normal;

    [Space(10), Header("Character Status")]
    [SerializeField] private int maxHp = 100;
    public int MaxHp => maxHp;
    private int currentHp = 100;
    public int CurrentHp 
    {
        get { return currentHp; }
        protected set 
        {
            currentHp = Mathf.Clamp(value, 0, maxHp);
            if (currentHp <= 0)
            {
                DeathEventArgs deathArgs = new DeathEventArgs(this);
                animator.SetTrigger("Death");
                BattleEventManager.OnDeath(deathArgs);
            }
        }
    }
    
    public int Level { get; protected set; } = 1;
    public int CurrentExp { get; protected set; } = 0;
    public int NextExp => CharacterLevelTable.GetCharacterGrowthData(Level).Exp;

    public bool IsDead => currentHp <= 0;
    protected bool IsAttacking = false;
    protected bool FinishedAction = false;

    public float DodgeActionTime { get; protected set; }= 0;
    public float ParryActionTime { get; protected set; }= 0;
    public float JumpActionTime { get; protected set; }= 0;

    
    public abstract TimelineAsset GetCurrentActionTimeline();
    protected abstract int GetCurrentDamage();

    protected abstract void OnDeath(DeathEventArgs args);

    protected virtual void OnEnable()
    {
        var callbacks = BattleEventManager.Callbacks;
        callbacks.OnAttack += OnAttack;
        callbacks.OnDeath += OnDeath;
    }

    protected virtual void OnDisable() {}

    protected virtual void Start()
    {
        currentHp = maxHp;
        StartCoroutine(UpdateDefendActionCoroutine());
    }
    
    protected virtual void Update()
    {
        if(Activated == false)
        {
            return;
        }

        if (IsDead == true)
        {
            return;
        }
        
        // Logic for character actions during the turn
    }
    
    protected abstract void OnTakedDamage();
    protected abstract void OnDodged();
    protected abstract void OnParried();
    protected abstract void OnJumped();

    

    private void OnAttack(AttackEventArgs args)
    {
        if (args.Target.Equals(this) == true)
        {
            Debug.Log($"{args.Attacker.name} attacked {name} for {args.Damage} damage.");
            DefendAttack(args.Damage, args.AttackTime, args.AttackType);
        }
    }
    
    public void DefendAttack(int damage, float attackTime, BattleAttackType attackType)
    {
        if (IsDead == true) return;

        switch (attackType)
        {
            case BattleAttackType.Normal:
            {
                if ((attackTime - DodgeActionTime) <= AttackDelay)
                {
                    DodgeEventArgs dodgeArgs = new DodgeEventArgs(this, attackTime);
                    BattleEventManager.OnDodge(dodgeArgs);
                    OnDodged();
                    return;
                }
                if ((attackTime - ParryActionTime) <= ParryDelay)
                {
                    ParryEventArgs parryArgs = new ParryEventArgs(this, attackTime);
                    BattleEventManager.OnParry(parryArgs);
                    OnParried();
                    return;
                }
                // 공격 적중
                TakeDamageEventArgs takeDamageArgs = 
                    new TakeDamageEventArgs(this, damage);
                
                animator.SetTrigger("Hit");
                BattleEventManager.OnTakeDamage(takeDamageArgs);
                OnTakedDamage();
                
                CurrentHp -= damage;
                break;
            }
            case BattleAttackType.Jump:
                if ((attackTime - JumpActionTime) <= AttackDelay)
                {
                    JumpEventArgs jumpArgs = new JumpEventArgs(this, attackTime);
                    BattleEventManager.OnJump(jumpArgs);
                    OnJumped();
                    return;
                }
                break;
            case BattleAttackType.Gradient:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(attackType), attackType, null);
        }
    }
  
    public void OnFocusIn() => characterCamera.Priority = 30;
    public void OnFocusOut() => characterCamera.Priority = 0;

    bool _activated = false;
    public bool Activated
    {
        get
        {
            return _activated;
        }
        protected set
        {
            if (value == true)
            {
                // 캐릭터가 활성화 되면 카메라 위선순위를 높혀 캐릭터에 포커싱 되도록한다.
                characterCamera.Priority = 20;
            }
            else
            {
                // 캐릭터가 비활성화 되면(턴 종료) 우선순위를 낮춰 자동으로 포커스 아웃되도록 처리한다.
                characterCamera.Priority = 0;
            }
            _activated = value;
        }
        
    }

    public virtual void Activate()
    {
        Activated = true;

    }
    
    public virtual void Deactivate()
    {
        Activated = false;
    }
    
    public void StartTurn()
    {
        Debug.Log($"BattleCharacter ::: StartTurn {name}");
        Activate();
        StartCoroutine(UpdateBattleActionCoroutine());
    }

    protected abstract IEnumerator UpdateBattleActionCoroutine();
    protected abstract IEnumerator UpdateDefendActionCoroutine();
    public IEnumerator AttackCoroutine()
    {
        // 1. TimelineAsset 가져오기
        var timeline = GetCurrentActionTimeline();
        yield return PlayTimeline(timeline,characterDefaultLocation, TargetCharacter.CharacterHitTransform);
        Deactivate();
        Debug.Log("타임라인 종료됨, 다음 단계 진행");
    }
    
    public IEnumerator WaitForTimeline(PlayableDirector director)
    {
        FinishedAction = false;

        void OnStopped(PlayableDirector _) => FinishedAction = true;

        director.stopped += OnStopped;

        // 이미 재생 중인 경우만 대기
        if (director.state == PlayState.Playing)
            yield return new WaitUntil(() => FinishedAction);

        director.stopped -= OnStopped;
    }

    public IEnumerator PlayTimeline(
        TimelineAsset timeline,
        Transform origin,
        Transform destination
        )
    {
        if (director.state == PlayState.Playing)
        {
            director.Stop();
            yield return null;
        }
        
        director.playableAsset = timeline;
        // 2. 모든 트랙 순회
        foreach (var track in timeline.GetOutputTracks())
        {
            if (track is AnimationTrack animationTrack)
            {
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
                            director.SetReferenceValue(moveClip.actor.exposedName, origin);
                            director.SetReferenceValue(moveClip.target.exposedName, destination);
                        }
                        else if (clip.displayName.Equals("ReturnTo"))
                        {
                            director.SetReferenceValue(moveClip.actor.exposedName, destination);
                            director.SetReferenceValue(moveClip.target.exposedName, origin);
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
                Debug.Log("BattleSignalEmitTrack found, binding to TimelineEventListener.");
                director.SetGenericBinding(track, TimelineEventListener.Instance);
            }
        }

        director.Play();
        yield return WaitForTimeline(director);
    }

}
