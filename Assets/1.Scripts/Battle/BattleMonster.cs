using System.Collections;
using UnityEngine;

public class BattleMonster : BattleCharacter
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override IEnumerator UpdateBattleActionCoroutine()
    {
        yield return StartCoroutine(SelectTargetCoroutine());
        
        yield return StartCoroutine(AttackCoroutine());
        
        
        Activated = false;
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
    
    private IEnumerator AttackCoroutine()
    {
        while (Input.GetKeyDown(KeyCode.Space) == false)
        {
            yield return null;
        }
        BattleEventManager.OnAttack(new AttackEventArgs(10, this, targetCharacter));
        
        Activated = false; 
    }
}
