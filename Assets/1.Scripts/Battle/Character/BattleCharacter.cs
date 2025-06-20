using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;


[System.Serializable]
public enum BattleCharacterType
{
    Player,
    Enemy
}

public enum BattleAttackType
{
    Normal,
    Skill,
    Jump,
    Gradient,
}

public abstract partial class BattleCharacter : MonoBehaviour
{
    const float AttackDelay = 0.33f; // Delay before the monster can attack again
    const float ParryDelay = 0.16f; // Delay before the monster can attack again
    
    [Header("Battle Character Settings")]
    [SerializeField] protected string characterName;
    [SerializeField] protected CharacterStatus status = new CharacterStatus();
    public CharacterStatus Status => status;

    [SerializeField] protected List<BattleAttribute> Attributes = new List<BattleAttribute>();
    public BattleAttribute GetAttribute(string name)
    {
        return Attributes.Find(attr => attr.AttributeName.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
    
    [SerializeField] protected List<string> registedEffectorTypes = new List<string>();
    protected Dictionary<string, StatusEffector> statusEffects = new Dictionary<string, StatusEffector>();
    public Dictionary<string, StatusEffector> StatusEffects => statusEffects;
    
    public GameStat Stat(string statName) { return status.GetStat(statName); }
    
    public string CharacterName => characterName;
    
    [Header("Character Location Settings")]
    [SerializeField] protected float focusRadius = 1f;
    public float FocusRadius { get { return focusRadius; } }
    
    [Space(10), Header("Character Action Settings")]
    [SerializeField] protected BattleActionController currentAction;
    public TimelineActor Actor =>  currentAction.Actor;  
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
            target: Target
        ));
    }
    
    protected void BindState(string stateName, CharacterState state)
    {
        if (StateMap.ContainsKey(stateName.ToLower()))
        {
            Debug.LogWarning($"StateMap already contains key: {stateName}");
            return;
        }
        StateMap[stateName.ToLower()] = state;
        state.Initialize(this);
    }
    
    public abstract void OnBeginDefendSignal();
    public abstract void OnEndDefendSignal();
    public abstract void OnCheckParriedSignal();
    
    public virtual void OnCounterAttackSignal() {}

    public abstract  BattleCharacterType CharacterType { get; }
    public abstract BattleCharacter Target { get; }
    
    public BattleAttackType CurrentAttackType { get; set; } =  BattleAttackType.Normal;  
    public string CurrentAttackAction { get; set; } =  "attack";  

    private string nextAction = "wait";
    protected string NextAction {
        get => nextAction;
        set{ nextAction = value.ToLower(); }
    }

    public int AttackCount { get; protected set; }= 0;
    
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

    protected abstract int GetCurrentDamage();

    
    protected CharacterState currentState;
    public CharacterState CurrentState
    {
        get => currentState;
        set
        {
            currentState = value;
            Debug.Log($"CurrentState changed to {currentState}");
        }
    }
    
    public void ReserveSwapState(string nextState)
    {
        nextState = nextState.ToLower();
        currentState.ReserveSwap(nextState);
    }
    
    public void SwapState(string nextState, bool immediate = true)
    {
        nextState = nextState.ToLower();
        if(currentState != null)
        {
            currentState.Exit();
        }

        Debug.Log($"<color=green>{gameObject}</color> Swapping state to {nextState}");
        currentState = StateMap[nextState];
        currentState.Enter();
    }

    public void SwapAction(string actionType, Action<PlayableDirector> callback = null)
    {
        actionType = actionType.ToLower();
        Debug.Log($"Swapping action to {actionType}");
        if (currentAction != null)
        {
            currentAction.gameObject.SetActive(false);
            currentAction.StopAction();
        }
        currentAction = ActionMap[actionType];
        currentAction.gameObject.SetActive(true);
        PlayActionArgs args = new PlayActionArgs(actor: this, target: Target, callback: callback);
        ActionMap[actionType].PlayAction(args);
    }
    
    public Dictionary<string, BattleActionController> ActionMap 
        = new Dictionary<string, BattleActionController>();

    public Dictionary<string, CharacterState> StateMap 
        = new Dictionary<string, CharacterState>();
    
    public virtual void Initialize()
    {
        var hp = status.GetStat(GameStat.HEALTH);
        hp.StatValue = hp.MaxValue;
        foreach (var data in actionLUT.actionDataList)
        {
            var actionController = Instantiate(data,transform);
            ActionMap[data.ActionName.ToLower()] = actionController;     
        }

        
        var callbacks = BattleEventManager.Callbacks;
        callbacks.OnAttack += OnAttack;
    }

    
    protected virtual void Start()
    {
        Initialize();
        SwapState("wait");

        foreach (var effectorAsset in AssetManager.Instance.StatusEffectorAssetList)
        {
            var key = effectorAsset.EffectorName.ToLower();
            statusEffects[key] = effectorAsset.CreateEffector();
        }
        
        foreach (var effector in registedEffectorTypes)
        {
            var key = effector.ToLower();
            statusEffects[key].EffectorValue = Random.Range(5,10);;
        }
    }

    protected void Update()
    {
        currentState.Execute();
    }

    protected virtual void OnTakedDamage(int damage)
    {
        if (IsDead)
        {
            return; // already dead, no damage can be taken
        }
        status.CurrentHP -= damage;
        if (IsDead)
        {
            // todo: 죽음 애니메이션 발동
            DeathEventArgs deathArgs = new DeathEventArgs(this);
            BattleEventManager.OnDeath(deathArgs);
            SwapState("death");
        }
    }
    
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
            case BattleAttackType.Skill:
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
  
    public virtual void OnFocusIn() {}

    public virtual void OnFocusOut() {}

    public virtual void Activate()
    {
        Activated = true;
        SwapState("active");
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

}
