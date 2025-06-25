using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleManager : MonoSingleton<BattleManager>
{
    [SerializeField] GameResultUI gameResultUI;
    [SerializeField] Transform[] playerSpawnPoints;
    [SerializeField] Transform[] enemySpawnPoints;
    
    private bool IsEndBattle { get; set; } = false;

    [SerializeField] private BattleCharacter[] battleCharacters;
    private readonly Queue<BattleCharacter> battlePriorityQueue = new Queue<BattleCharacter>();

    private readonly Dictionary<BattleCharacterLayer, List<BattleCharacter>> characterGroup 
        = new Dictionary<BattleCharacterLayer, List<BattleCharacter>>();
    
    public Dictionary<BattleCharacterLayer, List<BattleCharacter>> CharacterGroup
    {
        get { return characterGroup; }
    }
    
    public List<BattleCharacter> AlivedCharacterGroup(BattleCharacterLayer layer)
    {
        if (characterGroup.TryGetValue(layer, out var characterList))
        {
            var alivedCharacterList = characterList.Where(c => c.IsDead == false).ToList();
            return characterList;
        }
        return null;
    }

    public BattleCharacter CurrentTurnCharacter
    {
        get;
        private set;
    } = null;
    
    void Start()
    {
        Setup(GameUser.Instance);
    }

    public void Setup(GameUser user)
    {
        for(int i = 0; i < user.playerSamples.Count; i++)
        {
            var sample = user.playerSamples[i];
            var spawnPoint = playerSpawnPoints[i];
            var player = Instantiate(sample, spawnPoint.position, spawnPoint.rotation);
            var status = user.GetPlayerStatus(player.CharacterName);
            player.playerStatus = status;
            player.gameObject.SetActive(true);
            player.ReadyBattle();
            
            battlePriorityQueue.Enqueue(player);
            
            if (characterGroup.TryGetValue(player.CharacterLayer, out var characterList))
            {
                characterList.Add(player);
            }
            else
            {
                characterGroup[player.CharacterLayer] = new List<BattleCharacter> { player };
            }
        }

        for (int i = 0; i < user.enemySamples.Count; i++)
        {
            var sample = user.enemySamples[i];
            var spawnPoint = enemySpawnPoints[i];
            var enemy = Instantiate(sample, spawnPoint.position, spawnPoint.rotation);
            enemy.gameObject.SetActive(true);
            
            battlePriorityQueue.Enqueue(enemy);
            enemy.ReadyBattle();

            if (characterGroup.TryGetValue(enemy.CharacterLayer, out var characterList))
            {
                characterList.Add(enemy);
            }
            else
            {
                characterGroup[enemy.CharacterLayer] = new List<BattleCharacter> { enemy };
            }
        }
        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(UpdateBattleLoopCoroutine());
    }
    
    void CheckEndTurn()
    {
        var players = characterGroup[BattleCharacterLayer.Player];
        int playerDeadCount = 0;
        
        foreach (var player in players)
            if (player.IsDead) playerDeadCount++;
        
        if(playerDeadCount == players.Count)
        {
            IsEndBattle = true;
            Debug.Log("All players are dead. Battle ended.");
            GameManager.Instance.GoToWorldScene();
            return;
        }

        var enemies = characterGroup[BattleCharacterLayer.Monster];
        int enemyDeadCount = 0;
        
        foreach (var enemy in enemies)
            if (enemy.IsDead) enemyDeadCount++;

        if (enemyDeadCount == enemies.Count)
        {
            gameResultUI.gameObject.SetActive(true);
            gameResultUI.UpdateUI();
            IsEndBattle = true;
        }
        
        GameManager.Instance.EndBattle();
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
