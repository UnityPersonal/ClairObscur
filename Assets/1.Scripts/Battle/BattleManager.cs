using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleManager : MonoBehaviour
{
    private static BattleManager _instance;

    public static BattleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BattleManager>();
            }
            return _instance;
        }
    }
    
    private bool IsEndBattle { get; set; } = false;

    [SerializeField] private BattleCharacter[] battleCharacters;
    private readonly Queue<BattleCharacter> battlePriorityQueue = new Queue<BattleCharacter>();

    private readonly Dictionary<BattleCharacterType, List<BattleCharacter>> characterGroup 
        = new Dictionary<BattleCharacterType, List<BattleCharacter>>();
    
    public Dictionary<BattleCharacterType, List<BattleCharacter>> CharacterGroup
    {
        get { return characterGroup; }
    }
    
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
        foreach (var group in characterGroup)
        {
            for (int i = 0; i < group.Value.Count; i++)
            {
                
            }
        }
        
        // Logic to check if the battle has ended, e.g., all enemies defeated or player defeated
        // If the battle ends, set IsEndBattle to true
        // Example:
        // if (allEnemiesDefeated || playerDefeated)
        // {
        //     IsEndBattle = true;
        // }
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
                BattleCharacter currentTureCharacter = battlePriorityQueue.Dequeue();
                yield return StartCoroutine(UpdateTurnCoroutine(currentTureCharacter));
                battlePriorityQueue.Enqueue(currentTureCharacter);
            }
            
            CheckEndTurn();
            yield return null;
        }
        
    }


    IEnumerator UpdateTurnCoroutine(BattleCharacter currentTureCharacter = null)
    {
        Debug.Log("BattleManager ::: UpdateTurnCoroutine");
        
        if (currentTureCharacter == null)
        {
            Debug.LogError("BattleManager ::: UpdateTurnCoroutine - currentTureCharacter is null");
            yield break;
        }

        currentTureCharacter.StartTurn();
        while (currentTureCharacter.Activated == false)
        {
            // Logic to update the turn, e.g., processing player actions, enemy actions, etc.
            yield return null;
        }

    }
}
