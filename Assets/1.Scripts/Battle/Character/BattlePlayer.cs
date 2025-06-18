using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

public class BattlePlayer : BattleCharacter
{
    [Header("Battle Player Settings")]
    [SerializeField] Transform dodgeTransform;
    public Transform DodgeTransform { get { return dodgeTransform; } }
    [SerializeField] SkillData[] skills;
   
    private int parriedCount = 0;
    protected bool BeginParryingAttack = false;

    protected override void OnDodged() {}
    
    protected override void OnParried() { parriedCount++; }

    protected override void OnJumped() {}

    protected override int GetCurrentDamage()
    {
        return 10; // todo: 스킬, 장비, 상태, 직업, 데미지 연동
    }

    public BattleCharacter PlayerTargetCharacter { get; set; }= null;

   public override void OnBeginDefendSignal()
    {
        Debug.Log("OnEmittedBeginDefendSignal");
        parriedCount = 0;
        SwapState("defend");
    }

    public override void OnEndDefendSignal()
    {
        Debug.Log("OnEmittedEndDefendSignal");
        var attackCount = BattleManager.Instance.CurrentTurnCharacter.AttackCount;
        if (attackCount == parriedCount)
        {
            SwapState("counter");
        }
        else
        {
            SwapState("wait");
        }
    }

    public override void OnCheckParriedSignal() {}

    public override void Initialize()
    {
        base.Initialize();
        var stateContainer = new GameObject("PlayerStates");
        stateContainer.transform.SetParent(transform);
        
        BindState("attack", new PlayerAttackState());
        BindState("mainmenu", new PlayerMainMenuState());
        BindState("skillselect", new PlayerSkillSelectState());
        BindState("targetselect", new PlayerTargetSelectState());
        BindState("defend", new PlayerDefenseState());
        BindState("wait", new PlayerWaitState());
        BindState("active", new PlayerActiveState());
        BindState("counter", new PlayerCounterState());
        BindState("skillactive", new PlayerSkillActiveState());
        
        SwapState("wait");
    }

    int GetCoutnerDamage()
    {
        return GetCurrentDamage() * 2;
    }
    
    public override void OnCounterAttackSignal()
    {
        Debug.Log($"<color=purple>BattlePlayer</color> ::: OnEmittedCounterAttackSignal {Time.time}");
        CounterEventArgs counterArgs = new CounterEventArgs
        (
            attacker: this,
            target: BattleManager.Instance.CurrentTurnCharacter,
            damage: GetCoutnerDamage()
        );
        BattleEventManager.OnCounter(counterArgs);
    }

    public override BattleCharacterType CharacterType => BattleCharacterType.Player;
    public override BattleCharacter Target => PlayerTargetCharacter;

    
    
    private IEnumerator SkillActiveCoroutine()
    {
        while (IsDead == false)
        {
            yield return null;
        }
        Activated = false;
    }
    
    
    

    
}
