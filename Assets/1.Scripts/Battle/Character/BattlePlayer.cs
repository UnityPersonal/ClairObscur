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
    public enum StateType
    {
        None,
        Active,
        MainMenu,
        Attack,
        SkillSelect,
        TargetSelect,
        SkillActive,
        Defend,
        Wait
    }
    private bool isDefending = false;
    private int parriedCount = 0;
    protected bool BeginParryingAttack = false;

    private readonly Dictionary<StateType, PlayerState> playerStatesTable 
        = new Dictionary<StateType, PlayerState>();

    protected override void OnDodged() {}
    
    protected override void OnParried() { parriedCount++; }

    protected override void OnJumped() {}

    public override void Activate()
    {
        base.Activate();
        Debug.Log($"<color=green>BattlePlayer</color> ::: Activate {Time.time}");
        FinishedAction = true;
        nextState = StateType.Active;
    }
    
    public override void Deactivate()
    {
        base.Deactivate();
        nextState = StateType.Wait;
    }

    public override TimelineAsset GetCurrentActionTimeline()
    {
        switch (CurrentAttackType)
        {
            case BattleAttackType.Normal:
                var actionData = actionLUT.GetActionData(ActionDataType.Attack);
                return actionData.controller.timelineAsset;
            case BattleAttackType.Skill1 : return skills[0].timeline;
            case BattleAttackType.Skill2 : return skills[1].timeline;
            case BattleAttackType.Skill3 : return skills[2].timeline;
            case BattleAttackType.Jump:
                break;
            case BattleAttackType.Gradient:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return null;
    }

    protected override int GetCurrentDamage()
    {
        return 10; // todo: 스킬, 장비, 상태, 직업, 데미지 연동
    }

    public StateType PreviousActionType { get; private set; } = StateType.Wait;

    private StateType nextState = StateType.Wait;
    public StateType NextState
    {
        get => nextState;
        set
        {
            PreviousActionType = nextState;
            nextState = value;
        }
    }
    public BattleCharacter PlayerTargetCharacter { get; set; }= null;

   public override void OnBeginDefendSignal()
    {
        Debug.Log("OnEmittedBeginDefendSignal");
        isDefending = true;
        parriedCount = 0;
    }

    public override void OnEndDefendSignal()
    {
        Debug.Log("OnEmittedEndDefendSignal");
        isDefending = false;
        var attackCount = BattleManager.Instance.CurrentTurnCharacter.AttackCount;
        if (attackCount == parriedCount)
        {
            FinishedAction = true;
            BeginParryingAttack = true;
        }
    }

    public override void OnCheckParriedSignal() {}

    public override void Initialize()
    {
        base.Initialize();
        var stateContainer = new GameObject("PlayerStates");
        stateContainer.transform.SetParent(transform);
        
        playerStatesTable[StateType.Attack] = stateContainer.AddComponent<PlayerAttackState>();
        playerStatesTable[StateType.MainMenu] = stateContainer.AddComponent<PlayerMainMenuState>();
        playerStatesTable[StateType.SkillSelect] = stateContainer.AddComponent<PlayerSkillSelectState>();
        playerStatesTable[StateType.TargetSelect] = stateContainer.AddComponent<PlayerTargetSelectState>();
        playerStatesTable[StateType.Defend] = stateContainer.AddComponent<PlayerDefenseState>();
        playerStatesTable[StateType.Wait] = stateContainer.AddComponent<PlayerWaitState>();
        playerStatesTable[StateType.Active] = stateContainer.AddComponent<PlayerActiveState>();
        
        SwapAction(ActionDataType.Wait);
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
    public override BattleCharacter TargetCharacter => PlayerTargetCharacter;

    protected override IEnumerator UpdateBattleActionCoroutine()
    {
        while (gameObject.activeInHierarchy && (IsDead== false))
        {
            var state = playerStatesTable[NextState];
            yield return state.Execute(this);
        }
    }

    protected override IEnumerator UpdateDefendActionCoroutine()
    {
        while (gameObject.activeInHierarchy)
        {
            if(isDefending == false)
            {
                // 방어 중이 아닐 때는 대기
                yield return null;
                continue;
            }
            PlayerDefenseState state = playerStatesTable[StateType.Defend] as PlayerDefenseState;
            // dodge
            if (Input.GetKey(KeyCode.Q))
            {
                // 방어 중인 상태에서 처리할 로직을 여기에 작성합니다.
                // 예를 들어, 방어 애니메이션을 재생하거나 방어 상태를 표시하는 UI 업데이트 등을 할 수 있습니다.
                Debug.Log($"{name} is dodge.");
                state.defenseType = PlayerDefenseState.DefenseType.Dodge;
                yield return state.Execute(this);
                
            }
            // parry
            else if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log($"{name} is parrying.");
                state.defenseType = PlayerDefenseState.DefenseType.Parry;
                yield return state.Execute(this);

                if (BeginParryingAttack == true)
                {
                    BeginParryingAttack = false;
                    state.defenseType = PlayerDefenseState.DefenseType.ParryAttack;
                    yield return state.Execute(this);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log($"{name} is jumping.");
                state.defenseType = PlayerDefenseState.DefenseType.Jump;
                yield return state.Execute(this);
            }
            
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
