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
    [SerializeField] private PlayerStat playerStat = new PlayerStat();
    public PlayerStat PlayerStat { get { return playerStat; } }
    
    [SerializeField] Transform dodgeTransform;
    public Transform DodgeTransform { get { return dodgeTransform; } }
    [SerializeField] protected SkillDatabase skillDatabase;
    [SerializeField] protected string[] equippedSkills = new string[3]{"", "", ""};
    private int parriedCount = 0;
    protected bool BeginParryingAttack = false;

    public SkillController skillController;

    public override void Activate()
    {
        base.Activate();
    }
    
    public override void Deactivate()
    {
        base.Deactivate();
        PlayerStatUI.Instance.gameObject.SetActive(false);
    }

    private void OnRecivedTakedDamage(TakeDamageEventArgs args)
    {
        if (Activated == false) return;
        if (args.Target != Target) return;

        playerStat.currentAP += 1;
    }

    protected override void OnDodged()
    {
        playerStat.currentAP += 1;
    }

    protected override void OnParried()
    {
        parriedCount++;
        playerStat.currentAP += 1;

    }

    protected override void OnJumped() { playerStat.currentAP += 1; }

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
        for(int i = 0; i < equippedSkills.Length; i++)
        {
            string actionType = $"skill{i+1}".ToLower();
            
            var skillData = skillDatabase.GetSkillData(equippedSkills[i]);
            var actionController = Instantiate(skillData.action, transform);
            ActionMap[actionType] = actionController;

            if (ActionMap.ContainsKey(actionType) == false)
            {
                Debug.LogWarning($"ActionMap does not contain key: {actionType}");
                continue;
            }
        }
        
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
