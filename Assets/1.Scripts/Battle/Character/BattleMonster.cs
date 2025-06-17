using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BattleMonster : BattleCharacter
{
    protected override void OnEnable()
    {
        base.OnEnable();
        BattleEventManager.Callbacks.OnCounter += OnCounter;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        BattleEventManager.Callbacks.OnCounter -= OnCounter;
    }
    
    public override TimelineAsset GetCurrentActionTimeline()
    {
        var actionData = actionLUT.GetActionData(ActionDataType.Attack);
        return actionData.actionTimeline;
    }

    protected override int GetCurrentDamage()
    {
        return 10; // todo: 스킬, 상태, 몬스터 특성, 데미지 연동
    }

    private void OnCounter(CounterEventArgs args)
    {
        Debug.Log($"<color=purple>BattlePlayer</color> ::: OnCounter{Time.time}");
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
    public override void OnCheckParriedSignal() {}
    
    public void OnCounterBeginSignal()
    {
        currentAction.director.Pause();
        Debug.Log($"<color=purple>BattleMonster</color> ::: OnCounterBeginSignal {Time.time}");
    }
    
    public void OnCounterEndSignal()
    {
        currentAction.director.Resume();
        Debug.Log($"<color=purple>BattleMonster</color> ::: OnCounterEndSignal {Time.time}");
    }

    public override BattleCharacterType CharacterType => BattleCharacterType.Enemy;

    private BattleCharacter playerTargetCharacter = null;
    public override BattleCharacter TargetCharacter
    {
        get { return playerTargetCharacter; }
    }

    protected override IEnumerator UpdateBattleActionCoroutine()
    {
        while (gameObject.activeInHierarchy && (IsDead == false))
        {
            if(Activated == false)
            {
                yield return WaitCoroutine();
            }
            else
            {
                yield return StartCoroutine(SelectTargetCoroutine());
                yield return StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected override IEnumerator UpdateDefendActionCoroutine()
    {
        // 몬스터는 별도의 방어 액션이 없으므로 스킵한다.
        yield break;
    }


    private IEnumerator SelectTargetCoroutine()
    {
        var playerList = BattleManager.Instance.CharacterGroup[BattleCharacterType.Player];
        foreach (var character in playerList)
        {
            if(character.IsDead == false)
            {
                playerTargetCharacter = character;
                break;
            }
        }
        
        while (Input.GetKeyDown(KeyCode.Space) == false)
        {
            yield return null;
        }
    }
    
}
