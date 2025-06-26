using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BattleMonster : BattleCharacter
{
    [Space(20)]
    [Header("Battle Monster Settings")]
    [SerializeField] public CharacterStatus monsterStatus = new CharacterStatus();
    
    
    [SerializeField]
    private CinemachineCamera focusCamera;
    
    public override CharacterStatus Status => monsterStatus;
    
    public override void Initialize()
    {
        base.Initialize();
        BattleEventManager.Callbacks.OnCounter += OnCounter;
        
        BindState("wait", new MonsterWaitState());
        BindState("active", new MonsterActiveState());
        BindState("attack", new MonsterAttackState());
        BindState("death", new MonsterDeathState());
    }
    
    public override void OnFocusIn()
    {
        base.OnFocusIn();
        focusCamera.Priority = 30;
    }
    
    public override void OnFocusOut()
    {
        base.OnFocusOut();
        focusCamera.Priority = -10;
    }

    protected override int GetCurrentDamage()
    {
        return 10; // todo: 스킬, 상태, 몬스터 특성, 데미지 연동
    }

    private void OnCounter(CounterEventArgs args)
    {
        if (args.Target.Equals(this) == true)
        {
            Debug.Log($"<color=red>BattleMonster</color> ::: OnCounter {Time.time} {args.Damage}");
            TakeDamageEventArgs takeDamageArgs = new TakeDamageEventArgs
            (
                damage: args.Damage,
                target: this
            );
            BattleEventManager.OnTakeDamage(takeDamageArgs);
            OnTakedDamage(args.Damage);
        }
    }

    public override void OnBeginDefendSignal() {}
    public override void OnEndDefendSignal() {}
    
    public void OnCounterBeginSignal()
    {
        currentAction.PauseAction();
        Debug.Log($"<color=purple>{CharacterName}</color> ::: OnCounterBeginSignal {Time.time}");
    }
    
    public void OnCounterEndSignal()
    {
        currentAction.ResumeAction();
        Debug.Log($"<color=purple>{CharacterName}</color> ::: OnCounterEndSignal {Time.time}");
    }

    public override BattleCharacterLayer CharacterLayer => BattleCharacterLayer.Monster;

    public BattleCharacter playerTargetCharacter = null;
    public override BattleCharacter Target
    {
        get { return playerTargetCharacter; }
    }

}
