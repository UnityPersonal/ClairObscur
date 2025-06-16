using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BattleMonster : BattleCharacter
{
    public override TimelineAsset GetCurrentActionTimeline()
    {
        var actionData = actionLUT.GetActionData(ActionDataType.Attack);
        return actionData.actionTimeline;
    }

    protected override int GetCurrentDamage()
    {
        return 10; // todo: 스킬, 상태, 몬스터 특성, 데미지 연동
    }

    protected override void OnDeath(DeathEventArgs args)
    {
    }

    protected override void OnTakedDamage()
    {
    }

    protected override void OnDodged()
    {
    }

    protected override void OnParried()
    {
    }

    protected override void OnJumped()
    {
    }

    // Update is called once per frame
    public override void OnBeginAttackSignal()
    {
        Debug.Log($"<color=green>BattleMonster</color> ::: OnEmittedBeginAttackSignal {Time.time}");
        BattleEventManager.OnAttack( new AttackEventArgs
        (
            damage : GetCurrentDamage(),
            attackTime: Time.time,
            attackType: CurrentAttackType, // todo: 공격 종류에 따라 다르게 설정
            attacker: this,
            target: playerTargetCharacter
        ));
    }

    public override void OnBeginDefendSignal() {}
    public override void OnEndDefendSignal() {}
    public override void OnCheckParriedSignal() {}

    public override BattleCharacterType CharacterType => BattleCharacterType.Enemy;

    private BattleCharacter playerTargetCharacter = null;
    public override BattleCharacter TargetCharacter
    {
        get { return playerTargetCharacter; }
    }

    protected override IEnumerator UpdateBattleActionCoroutine()
    {
        yield return StartCoroutine(SelectTargetCoroutine());
        
        yield return StartCoroutine(AttackCoroutine());
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
