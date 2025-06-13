using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleManager : MonoSingleton<BattleManager>
{
    [SerializeField] GameResultUI gameResultUI;
    
    private bool IsEndBattle { get; set; } = false;

    [SerializeField] private BattleCharacter[] battleCharacters;
    private readonly Queue<BattleCharacter> battlePriorityQueue = new Queue<BattleCharacter>();

    private readonly Dictionary<BattleCharacterType, List<BattleCharacter>> characterGroup 
        = new Dictionary<BattleCharacterType, List<BattleCharacter>>();
    
    public Dictionary<BattleCharacterType, List<BattleCharacter>> CharacterGroup
    {
        get { return characterGroup; }
    }

    public BattleCharacter CurrentTurnCharacter
    {
        get;
        private set;
    } = null;
    
    void Start()
    {
        foreach (var character in battleCharacters)
        {
            if (character == null)
            {
                Debug.LogError("BattleManager ::: Start - BattleCharacter is null");
                continue;
            }
            battlePriorityQueue.Enqueue(character);

            if (characterGroup.TryGetValue(character.CharacterType, out var characterList))
            {
                characterList.Add(character);
            }
            else
            {
                characterGroup[character.CharacterType] = new List<BattleCharacter> { character };
            }
        }
        
        StartCoroutine(UpdateBattleLoopCoroutine());
    }
    
    void CheckEndTurn()
    {
        var players = characterGroup[BattleCharacterType.Player];
        int playerDeadCount = 0;
        
        foreach (var player in players)
            if (player.IsDead) playerDeadCount++;
        
        if(playerDeadCount == players.Count)
        {
            IsEndBattle = true;
            Debug.Log("All players are dead. Battle ended.");
            return;
        }

        var enemies = characterGroup[BattleCharacterType.Enemy];
        int enemyDeadCount = 0;
        
        foreach (var enemy in enemies)
            if (enemy.IsDead) enemyDeadCount++;

        if (enemyDeadCount == enemies.Count)
        {
            gameResultUI.gameObject.SetActive(true);
            gameResultUI.UpdateUI();
            IsEndBattle = true;
        }
    }

    IEnumerator UpdateBattleLoopCoroutine()
    {
        yield return null;
        
        while (IsEndBattle == false)
        {
            // 현재 턴의 캐릭터를 활성화 시킨다.
            // 플레이어는 수동 선택 액션,
            // 적은 자동 선택 액션을 수행한다.
            if (battlePriorityQueue.Count > 0)
            {
                CurrentTurnCharacter = battlePriorityQueue.Dequeue();
                if (CurrentTurnCharacter.IsDead == false)
                {
                    yield return StartCoroutine(UpdateTurnCoroutine());
                    battlePriorityQueue.Enqueue(CurrentTurnCharacter);    
                }
            }
            
            CheckEndTurn();
            yield return null;
        }
        
    }


    IEnumerator UpdateTurnCoroutine()
    {
        CurrentTurnCharacter.StartTurn();
        while (CurrentTurnCharacter.Activated == true)
        {
            // Logic to update the turn, e.g., processing player actions, enemy actions, etc.
            yield return null;
        }

    }
}
