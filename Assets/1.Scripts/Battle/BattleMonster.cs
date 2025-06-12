using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BattleMonster : BattleCharacter
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnAttack(AttackEventArgs args)
    {
        if(args.Target.Equals(this) == true)
        {
            Debug.Log($"{args.Attacker.name} attacked {name} for {args.Damage} damage.");
            animator.SetTrigger("Hit");
            
            BattleEventManager.OnTakeDamage(
                new TakeDamageEventArgs(this, args.Damage)
                );
        }
    }

    // Update is called once per frame
    public override void OnEmittedBeginAttackSignal()
    {
        BattleEventManager.OnAttack(new AttackEventArgs(10, this, targetCharacter));
    }

    public override void OnEmittedBeginDefendSignal()
    {
    }

    public override void OnEmittedEndDefendSignal()
    {
    }

    public override BattleCharacterType CharacterType => BattleCharacterType.Enemy;

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
        yield break;
    }

    private BattleCharacter targetCharacter = null;

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
        // Logic to select a target for the monster
        yield return null; // Placeholder for actual logic
    }
    
}
