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
    [SerializeField] SkillData[] skills;
    public enum ActionType
    {
        None,
        MainMenu,
        Attack,
        SkillSelect,
        TargetSelect,
        SkillActive,
    }
    private bool isDefending = false;

    private readonly Dictionary<ActionType, PlayerState> playerStatesTable 
        = new Dictionary<ActionType, PlayerState>();

    protected override void OnTakedDamage() {}
    
    protected override void OnDodged() {}
    
    protected bool BeginParryingAttack = false;
    protected override void OnParried()
    {
        FinishedAction = true;
        BeginParryingAttack = true;
    }

    protected override void OnJumped() {}

    public override TimelineAsset GetCurrentActionTimeline()
    {
        switch (CurrentAttackType)
        {
            case BattleAttackType.Normal:
                var actionData = actionLUT.GetActionData(ActionDataType.Attack);
                return actionData.actionTimeline;
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

    protected override void OnDeath(DeathEventArgs args) {}

    public ActionType PreviousActionType { get; set; } = ActionType.MainMenu;

    private ActionType nextAction = ActionType.MainMenu;
    public ActionType NextAction
    {
        get => nextAction;
        set
        {
            PreviousActionType = nextAction;
            nextAction = value;
        }
    }
    public BattleCharacter PlayerTargetCharacter { get; set; }= null;

    protected override void Start()
    {
        base.Start();
        var stateContainer = new GameObject("PlayerStates");
        playerStatesTable[ActionType.Attack] = stateContainer.AddComponent<PlayerAttackState>();
        playerStatesTable[ActionType.MainMenu] = stateContainer.AddComponent<PlayerMainMenuState>();
        playerStatesTable[ActionType.SkillSelect] = stateContainer.AddComponent<PlayerSkillSelectState>();
        playerStatesTable[ActionType.TargetSelect] = stateContainer.AddComponent<PlayerTargetSelectState>();
    }

    public override void OnBeginAttackSignal()
    {
        float attackTime = Time.time;
        BattleEventManager.OnAttack( new AttackEventArgs
        (
            damage : GetCurrentDamage(),
            attackTime: attackTime,
            attackType: CurrentAttackType,
            attacker: this,
            target: PlayerTargetCharacter
        ));
    }

    public override void OnBeginDefendSignal()
    {
        Debug.Log("OnEmittedBeginDefendSignal");
        isDefending = true;
    }

    public override void OnEndDefendSignal()
    {
        Debug.Log("OnEmittedEndDefendSignal");
        isDefending = false;
    }

    public override void OnCheckParriedSignal() {}

    public override BattleCharacterType CharacterType => BattleCharacterType.Player;
    public override BattleCharacter TargetCharacter => PlayerTargetCharacter;

    protected override IEnumerator UpdateBattleActionCoroutine()
    {
        NextAction =  ActionType.MainMenu; 

        while (Activated && !IsDead)
        {
            var state = playerStatesTable[NextAction];
            yield return StartCoroutine(state.Execute(this));
        }
        Debug.Log($"BattlePlayer Turn Ended");
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
            
            // dodge
            if (Input.GetKey(KeyCode.Q))
            {
                // 방어 중인 상태에서 처리할 로직을 여기에 작성합니다.
                // 예를 들어, 방어 애니메이션을 재생하거나 방어 상태를 표시하는 UI 업데이트 등을 할 수 있습니다.
                Debug.Log($"{name} is dodge.");
                yield return StartCoroutine(DodgeCoroutine());
                
            }
            // parry
            else if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log($"{name} is parrying.");
                yield return StartCoroutine(ParryingCoroutine());

                if (BeginParryingAttack == true)
                {
                    BeginParryingAttack = false;
                    yield return StartCoroutine(ParryingAttackCoroutine());
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log($"{name} is jumping.");
                yield return StartCoroutine(JumpingCoroutine());
                // 방어 애니메이션을 재생하거나 방어 상태를 표시하는 UI 업데이트 등을 할 수 있습니다.
            }
            
            yield return null;
        }
    }
    
    protected IEnumerator DodgeCoroutine()
    {
        var actionData =  actionLUT.GetActionData(ActionDataType.Dodge);
        var timeline = actionData.actionTimeline;
        DodgeActionTime = Time.time; // 가장 최근 회피 시간을 캐싱한다.
        Debug.Log($"<color=red>{gameObject.name} Dodge Play </color>");
        yield return PlayTimeline(timeline,characterDefaultLocation, dodgeTransform);
    }

    private IEnumerator ParryingCoroutine()
    {
        var actionData =  actionLUT.GetActionData(ActionDataType.Parry);
        var timeline = actionData.actionTimeline;
        ParryActionTime = Time.time;
        Debug.Log($"<color=blue>{gameObject.name} Parry Play </color>");
        yield return PlayTimeline(timeline, characterDefaultLocation, characterDefaultLocation);
    }

    private IEnumerator ParryingAttackCoroutine()
    {
        var actionData =  actionLUT.GetActionData(ActionDataType.ParryingAttack);
        var timeline = actionData.actionTimeline;
        Debug.Log($"<color=yellow>{gameObject.name} Parry Attack Play </color>");
        yield return PlayTimeline(timeline, characterDefaultLocation, characterDefaultLocation);
    }
    
    private IEnumerator JumpingCoroutine()
    {
        JumpActionTime = Time.time; // 가장 최근 점프 시간을 캐싱한다.
        yield break;
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
