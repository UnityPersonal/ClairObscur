using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
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
    
    [Header("Battle Player Settings")]
    [SerializeField] protected string characterName;
    [SerializeField] protected CharacterStat status = new CharacterStat();
    public CharacterStat Status => status;
    public string CharacterName => characterName;
    
    [Header("Character Location Settings")]
    [SerializeField] protected Transform characterDefaultLocation;
    public Transform CharacterDefaultLocation { get { return characterDefaultLocation; } }
    [SerializeField] protected Transform characterHitTransform;
    public Transform CharacterHitTransform { get { return characterHitTransform; } }
    
    [Space(10), Header("Character Action Settings")]
    [SerializeField] protected BattleActionController currentAction;
    [SerializeField] protected ActionDataTable actionLUT;

    public virtual void OnBeginAttackSignal()
    {
        Debug.Log($"<color=green>BattleCharacter</color> ::: OnEmittedBeginAttackSignal {Time.time}");
        AttackCount++;
        BattleEventManager.OnAttack( new AttackEventArgs
        (
            damage : GetCurrentDamage(),
            attackTime: Time.time,
            attackType: CurrentAttackType, // todo: 공격 종류에 따라 다르게 설정
            attacker: this,
            target: TargetCharacter
        ));
    }
    public abstract void OnBeginDefendSignal();
    public abstract void OnEndDefendSignal();
    public abstract void OnCheckParriedSignal();
    
    public virtual void OnCounterAttackSignal() {}

    public abstract  BattleCharacterType CharacterType { get; }
    public abstract BattleCharacter TargetCharacter { get; }
    
    public BattleAttackType CurrentAttackType { get; set; }= BattleAttackType.Normal;

    protected bool IsAttacking = false;
    public int AttackCount { get; protected set; }= 0;
    protected bool FinishedAction = false;
    
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
                AttackCount = 0;
            }
            else
            {
            }
            _activated = value;
        }
        
    }
    
    public bool IsDead => status.IsDead;

    public float DodgeActionTime { get; set; }= 0;
    public float ParryActionTime { get; set; }= 0;
    public float JumpActionTime { get; set; }= 0;

    
    public abstract TimelineAsset GetCurrentActionTimeline();
    protected abstract int GetCurrentDamage();

    public void SwapAction(ActionDataType actionType)
    {
        Debug.Log($"Swapping action to {actionType}");
        if (currentAction != null)
        {
            currentAction.director.Stop();
            currentAction.gameObject.SetActive(false);
        }
        currentAction = ActionMap[actionType];
        currentAction.gameObject.SetActive(true);
    }
    
    
    protected virtual void OnEnable()
    {
        var callbacks = BattleEventManager.Callbacks;
        callbacks.OnAttack += OnAttack;
        callbacks.OnDeath += OnDeath;
    }

    protected virtual void OnDisable()
    {
        var callbacks = BattleEventManager.Callbacks;
        callbacks.OnAttack -= OnAttack;
        callbacks.OnDeath -= OnDeath;
    }
    
    public Dictionary<ActionDataType, BattleActionController> ActionMap 
        = new Dictionary<ActionDataType, BattleActionController>();

    public virtual void Initialize()
    {
        status.CurrentHP = status.MaxHP;
        foreach (var data in actionLUT.actionDataList)
        {
            var actionController = Instantiate(data.controller,transform);
            ActionMap[data.actionDataType] = actionController;     
        }
    }

    
    protected virtual void Start()
    {
        Initialize();
        StartCoroutine(UpdateBattleActionCoroutine());
        StartCoroutine(UpdateDefendActionCoroutine());
    }
    
    protected virtual void OnTakedDamage(int damage)
    {
        if (IsDead == true)
        {
            return; // already dead, no damage can be taken
        }
        status.CurrentHP -= damage;
        if (IsDead == true)
        {
            // todo: 죽음 애니메이션 발동
            DeathEventArgs deathArgs = new DeathEventArgs(this);
            BattleEventManager.OnDeath(deathArgs);
        }
    }
    
    protected virtual void OnDeath(DeathEventArgs args) {}
    
    protected virtual void OnDodged() {}
    protected virtual void OnParried() {}
    protected virtual void OnJumped() {}

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
                
                // todo : 피격 애니메이션 실행
                BattleEventManager.OnTakeDamage(takeDamageArgs);
                OnTakedDamage(damage);
                
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
  
    public void OnFocusIn()
    {
        
    }

    public void OnFocusOut()
    {
        
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
    }

    protected abstract IEnumerator UpdateBattleActionCoroutine();
    protected abstract IEnumerator UpdateDefendActionCoroutine();
    
    public IEnumerator WaitCoroutine()
    {
        Debug.Log($"<color=blue>BattleCharacter</color> ::: WaitCoroutine {Time.time}");
        var timeline = ActionMap[ActionDataType.Wait].timelineAsset;
        yield return PlayTimeline(timeline,characterDefaultLocation, characterDefaultLocation);
    }
    public IEnumerator AttackCoroutine()
    {
        // 1. TimelineAsset 가져오기
        // todo: 현재 액션 타입에 따라 타임라인을 가져온다.
        var timeline = ActionMap[ActionDataType.Attack].timelineAsset;
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
        Transform destination,
        bool waitForCompletion = true
        )
    {
        var director = currentAction.director;
        director.playableAsset = timeline;
        // 2. 모든 트랙 순회
        foreach (var track in timeline.GetOutputTracks())
        {
            if (track is CinemachineTrack)
            {
                var brain = Camera.main.GetComponent<CinemachineBrain>();
                director.SetGenericBinding(track,brain);
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
                director.SetGenericBinding(track, TimelineEventRouter.Instance);
            }
        }

        director.Play();

        if (waitForCompletion)
        {
            yield return WaitForTimeline(director);
        }
    }

}
