using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BattleMonster : BattleCharacter
{
    
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
    public override void OnEmittedBeginAttackSignal()
    {
        int damage = 10;
        float attackTime = Time.time;
        
        BattleEventManager.OnAttack( new AttackEventArgs
        (
            damage : damage,
            attackTime: attackTime,
            attackType: BattleAttackType.Normal, // todo: 공격 종류에 따라 다르게 설정
            attacker: this,
            target: targetCharacter
        ));
    }

    public override void OnEmittedBeginDefendSignal() {}

    public override void OnEmittedEndDefendSignal() {}

    public override BattleCharacterType CharacterType => BattleCharacterType.Enemy;

    private BattleCharacter targetCharacter = null;
    public override BattleCharacter TargetCharacter
    {
        get { return targetCharacter; }
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
                targetCharacter = character;
                break;
            }
        }
        
        while (Input.GetKeyDown(KeyCode.Space) == false)
        {
            yield return null;
        }
    }
    
}
