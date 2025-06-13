using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BattleMonster : BattleCharacter
{
    const float AttackDelay = 0.33f; // Delay before the monster can attack again
    const float ParryDelay = 0.16f; // Delay before the monster can attack again
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnAttack(AttackEventArgs args)
    {
        if(args.Target.Equals(this) == true)
        {
            Debug.Log($"{args.Attacker.name} attacked {name} for {args.Damage} damage.");
            animator.SetTrigger("Hit");
            TakeDamage(args.Damage, args.Dodged, args.Parried, args.Jumped);
        }
    }

    private readonly List<float> dogeTimes = new List<float>();
    private readonly List<float> parryTimes = new List<float>();
    private readonly List<float> jumpTimes = new List<float>();
    
    protected override void OnDodge(DodgeEventArgs args)
    {
        // 방어 캐릭터가 회피를 발동한 타이밍을 캐싱해둔다.
        if (Activated == true)
        {
            dogeTimes.Add(args.Time);
        }
    }

    protected override void OnDeath(DeathEventArgs args)
    {
    }

    // Update is called once per frame
    public override void OnEmittedBeginAttackSignal()
    {
        int damage = 10;
        float attackTime = Time.time;
        bool isDodged = false;
        bool isParried = false;
        bool isJumped = false;
        foreach (float dodgeTime in dogeTimes)
        {
            if(attackTime - dodgeTime <= AttackDelay)
            {
                Debug.Log($"{name} dodged sucees!");
                damage = 0; // 공격 피해를 0으로 설정한다.
                isDodged = true;
                break;
                // todo: 패링 판정도 추가되어야 한다.
            }
        }
        
        BattleEventManager.OnAttack( new AttackEventArgs
        (
            damage : damage,
            attacker: this,
            target: targetCharacter,
            dodged: isDodged,
            parried: isParried,
            jumped: isJumped
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
        // Reset action states
        dogeTimes.Clear();
        parryTimes.Clear();
        jumpTimes.Clear();
        
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
