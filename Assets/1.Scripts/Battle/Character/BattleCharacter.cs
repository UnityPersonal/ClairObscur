using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;


[System.Serializable]
public enum BattleCharacterLayer
{
    Player,
    Monster,
}

public enum BattleAttackType
{
    Normal,
    Skill,
}

public abstract partial class BattleCharacter : MonoBehaviour
{
    public CallbackEvents Callbacks { get; private set; } = new CallbackEvents();
    
    [Header("Battle Character Settings")]
    public abstract CharacterStatus Status { get; } 
    public WeaponStatus weaponStatus = new WeaponStatus();

    List<StatusEffector> dealEffectors = new List<StatusEffector>();
    protected Dictionary<string, StatusEffector> statusEffects = new Dictionary<string, StatusEffector>();
    public Dictionary<string, StatusEffector> StatusEffects => statusEffects;
    
    public GameStat Stat(string statName) { return Status.GetStat(statName.ToLower()); }
    public StatusEffector StatusEffect(string statName) { return statusEffects[statName.ToLower()]; }
    public string CharacterName => Status.CharacterName;
    
    [Header("Character Cinematic Settings")]
    [SerializeField] protected float focusRadius = 1f;
    public float FocusRadius { get { return focusRadius; } }
    
    [Space(10), Header("Character Action Settings")]
    [SerializeField] protected BattleActionController currentAction;
    public TimelineActor Actor =>  currentAction.Actor;  
    [SerializeField] protected ActionDataTable actionLUT;
    
    public void PauseAction() { currentAction.PauseAction();}
    public void ResumeAction() {  currentAction.ResumeAction(); }
    
    public virtual void OnBeginAttackSignal()
    {
        Debug.Log($"<color=green>{CharacterName}</color> ::: OnEmittedBeginAttackSignal {Time.time}");
        AttackCount++;

        // todo: 무기의 속성 가져오기
        string attackEffector = weaponStatus.weaponDealEffector; // 기본 공격 효과
        foreach (var deal in dealEffectors)
        {
            if(deal.EffectorValue > 0)
            {
                attackEffector = deal.EffectorName;
                break;
            }
        }

        BattleEventManager.OnAttack( new AttackEventArgs
        (
            damage : GetCurrentDamage(),
            attackTime: Time.time,
            attackEffector: attackEffector, // todo: 공격 종류에 따라 다르게 설정
            attacker: this,
            target: Target
        ));
    }
    
    protected void BindState(string stateName, CharacterState state)
    {
        if (StateMap.ContainsKey(stateName.ToLower()))
        {
            Debug.LogWarning($"{CharacterName} StateMap already contains key: {stateName}");
            return;
        }
        StateMap[stateName.ToLower()] = state;
        state.Initialize(this);
    }
    
    public abstract void OnBeginDefendSignal();
    public abstract void OnEndDefendSignal();
    public virtual void OnCounterAttackSignal() {}

    public abstract  BattleCharacterLayer CharacterLayer { get; }
     
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
    
    public bool IsDead => Status.IsDead;

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
        Status.BindCharacter(this);
       
        var hp = Status.GetStat(GameStat.HEALTH);
        hp.StatValue = hp.MaxValue;
        foreach (var data in actionLUT.actionDataList)
        {
            var actionController = Instantiate(data,transform);
            ActionMap[data.ActionName.ToLower()] = actionController;     
        }

        
        var callbacks = BattleEventManager.Callbacks;
        callbacks.OnAttack += OnAttack;
    }
    
    public virtual void ReadyBattle()
    {
        Initialize();
        SwapState("wait");
        
        foreach (var effectorAsset in AssetManager.Instance.CommonEffectorList)
        {
            var key = effectorAsset.EffectorName.ToLower();
            var effector = effectorAsset.CreateEffector(this);
            statusEffects[key] = effector;
            
            effector.BindCharacter(this);
        }

        foreach (var effectorAsset in AssetManager.Instance.dealEffectorTable.AssetList)
        {
            var key = effectorAsset.EffectorName.ToLower();
            var effector = effectorAsset.CreateEffector(this);
            statusEffects[key] = effector;
            dealEffectors.Add(effector);
            
            effector.BindCharacter(this);
        }
        foreach (var effectorAsset in AssetManager.Instance.buffEffectorTable.AssetList)
        {
            var key = effectorAsset.EffectorName.ToLower();
            var effector = effectorAsset.CreateEffector(this);
            statusEffects[key] = effector;
            
            effector.BindCharacter(this);
        }
        
        WorldSpaceUISpawner.Instance.SpawnHpBar(this);
        WorldSpaceUISpawner.Instance.SpawnStatusEffectUI(this);
    }

    protected void Update()
    {
        currentState.Execute();
    }
    
    public void OnTakeHeal(int healAmount)
    {
        if (IsDead)
        {
            return; // already dead, no healing can be applied
        }

        if (StatusEffect("invert").EffectorValue > 0)
        {
            OnTakedDamage(healAmount);
        }
        else
        {
            var health = Status.GetStat(GameStat.HEALTH);
            health.IncrementStatValue(healAmount);
        }
        
        
    }

    protected virtual void OnTakedDamage(int damage)
    {
        if (IsDead)
        {
            return; // already dead, no damage can be taken
        }

        { // apply shield effect
            var shieldEffect = StatusEffect("shield");
            if (shieldEffect.EffectorValue > 0)
            {
                shieldEffect.EffectorValue = shieldEffect.EffectorValue - 1;
                return; // ignore damage if shield is active
            }
        }
        
        var killEffector = StatusEffect("kill");
        if (killEffector.EffectorValue > 0)
        { 
            var health = Status.GetStat(GameStat.HEALTH);
            float hpRatio = (float)health.StatValue / (float)health.MaxValue;
            float killRatio = killEffector.EffectorValue / 100f;
            if (hpRatio <= killRatio)
            { // apply kill effect
                damage = health.MaxValue;
            }
        }
        
        { // apply mark effct
            var markEffect = StatusEffect("mark");
            if (markEffect.EffectorValue > 0)
            {
                damage += (int)(damage * 0.5f); // increase damage by mark effect percentage
                markEffect.EffectorValue = 0; // reset mark effect after applying
            }
        }
        
        { // apply double hit effect
            var doubleHit = StatusEffect("doubleHit");
            if (doubleHit.EffectorValue > 0)
            {
                damage *= 2; // double the damage
            }
        }

        { // todo: apply break effect (break gauge stat 추가 필요)
            
        }
        
        // apply damage to character's health
        Status.CurrentHP -= damage;
        
        // 공격 적중
        TakeDamageEventArgs takeDamageArgs = new TakeDamageEventArgs(this, damage);
        // todo : 피격 애니메이션 실행
        BattleEventManager.OnTakeDamage(takeDamageArgs);
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
            DefendAttack(args.Damage, args.AttackTime, args.AttackEffector);
        }
    }
    
    public void DefendAttack(int damage, float attackTime, string attackType)
    {
        if (IsDead == true) return;

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
        
        switch (attackType)
        {
            case "fire":
                var burn = StatusEffect("burn");
                burn.EffectorValue = (burn.EffectorValue + 3);
                break;
            case "ice":
                var freeze = StatusEffect("freeze");
                freeze.EffectorValue = (freeze.EffectorValue + 3);
                break;
            case "physical":
            case "light":
            case "lightning":
            case "dark":
            case "void":
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(attackType), attackType, null);
        }
        OnTakedDamage(damage);

        Callbacks.OnStatusChanged?.Invoke();
        Callbacks.OnEffectorChanged?.Invoke();
        Callbacks.OnAttributeChanged?.Invoke();

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

    public void EndTurn()
    {
        {
            // end turn event
            EndTurnEventArgs args = new EndTurnEventArgs(character: this);
            BattleEventManager.OnEndTurn(args);
            Debug.Log($"{CharacterName} ::: EndTurn");
        }

        {
            // reset double hit 
            StatusEffect("doubleHit").EffectorValue = 0;
            StatusEffect("kill").EffectorValue = 0;
        }

        {
            // decrease effect
            StatusEffect("regen").EffectorValue = 0;
            StatusEffect("powerful").EffectorValue = 0;
            StatusEffect("rush").EffectorValue = 0;
            StatusEffect("shell").EffectorValue = 0;
            StatusEffect("powerless").EffectorValue = 0;
            StatusEffect("slow").EffectorValue = 0;
            StatusEffect("defenceless").EffectorValue = 0;
        }
    }

    public void StartTurn()
    {

        {// start turn event
            StartTurnEventArgs args = new StartTurnEventArgs(character: this);
            BattleEventManager.OnStartTurn(args);
            Debug.Log($"{CharacterName} ::: StartTurn");
        }

        {// apply curse effect
            var curseEffect = StatusEffect("curse");
            if (curseEffect.EffectorValue == 1)
            {
                var hp = Status.GetStat(GameStat.HEALTH);
                OnTakedDamage(hp.MaxValue);
                Debug.Log($"<color=red>{name}</color> ::: Curse effect applied," +
                          $" Character Die Immediatly");
                curseEffect.EffectorValue = 0; // reset curse effect
                return;
            }
            else if (curseEffect.EffectorValue > 1)
            {
                Debug.Log($"<color=red>{name}</color> ::: Curse effect applied, remaining value: {curseEffect.EffectorValue}");
                curseEffect.EffectorValue = (curseEffect.EffectorValue - 1); // reduce curse effect value
            }
        }
        
        {// apply burn effect
            var burnEffect = StatusEffect("burn");
            if (burnEffect.EffectorValue > 0)
            {
                OnTakedDamage(10 * burnEffect.EffectorValue);
                Debug.Log($"<color=red>{name}</color> ::: Burn effect applied, remaining value: {burnEffect.EffectorValue}");
                burnEffect.EffectorValue = (burnEffect.EffectorValue - 1); // reduce burn effect value
            }
        }
        
        {// stun effect
            var stunEffect = StatusEffect("stun");
            if (stunEffect.EffectorValue == 1)
            {
                Debug.Log($"<color=red>{name}</color> ::: Stun effect Value," +
                          $" Skip Turn");
                stunEffect.EffectorValue = 0;
                Deactivate();
                return;
            }
        }

        {// apply freeze effect
            var freezeEffect = StatusEffect("freeze");
            if (freezeEffect.EffectorValue > 0)
            {
                Debug.Log($"<color=red>{name}</color> ::: Freeze effect Value," +
                          $" Skip Turn");
                freezeEffect.EffectorValue = (freezeEffect.EffectorValue - 1);
                Deactivate();
                return;
            }
        }
        
        Activate();

        
    }

}
