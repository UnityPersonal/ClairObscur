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
    [SerializeField] Transform dodgeTransform;
    public Transform DodgeTransform { get { return dodgeTransform; } }
    [SerializeField] protected SkillDatabase skillDatabase;
    [SerializeField] protected string[] equippedSkills = new string[3]{"", "", ""};
    private int parriedCount = 0;
    protected bool BeginParryingAttack = false;

    public int CurrentSelectSkillIndex { get; set; } = 0;
    public SkillController skillController;
    
    public SkillData GetSkillDataByIndex(int index)
    {
        if (index < 0 || index >= equippedSkills.Length)
        {
            Debug.LogError($"Invalid skill index: {index}");
            return null;
        }
        
        return skillDatabase.GetSkillData(equippedSkills[index]);
    }

    public override void Activate()
    {
        base.Activate();
    }
    
    public override void Deactivate()
    {
        base.Deactivate();
        PlayerStatUI.Instance.gameObject.SetActive(false);
    }
    
    public void IncrementAP(int amount)
    {
        var ap = Stat(GameStat.AP);
        ap.SetStatValue(ap.StatValue + amount);
    }

    private void OnRecivedTakedDamage(TakeDamageEventArgs args)
    {
        if (Activated == false) return;
        if (args.Target != Target) return;

        IncrementAP(1);
    }

    protected override void OnDodged()
    {
        IncrementAP(1);
    }

    protected override void OnParried()
    {
        parriedCount++;
        IncrementAP(1);
    }

    protected override void OnJumped() { IncrementAP(1);}

    protected override int GetCurrentDamage()
    {
        var attackDamage = Stat(GameStat.ATTACK_POWER);
        // todo: 어빌리티 값을 데미지에 반영
        // todo: 스킬, 장비, 상태, 직업, 데미지 연동
        return attackDamage.StatValue ;
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
            // todo: 바로 돌아가는게 아니라 이전 애니메이션 재생 끝나면 넘어가도록 수정하자.
            ReserveSwapState("wait");
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
        BindState("death", new PlayerDeathState());

        
        return;
        
        
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

    public override BattleCharacterLayer CharacterLayer => BattleCharacterLayer.Player;
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
