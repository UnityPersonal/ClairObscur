using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : DontDestorySingleton<GameManager>
{
    
    const int LOBBY_SCENE_INDEX = 0;
    const int WORLD_SCENE_INDEX = 1;
    public const int BATTLE_SCENE_INDEX = 2;
    public const int BOSS_SCENE_INDEX = 3;

    private void StartGame(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Boss Battle Scene Loaded");
        BattleManager.Instance.StartGame();
        SceneManager.sceneLoaded -= StartGame;
    }
    
    public void StartBattle(List<BattleMonster> battleCharacters, string battleScenePath)
    {
        // 배틀씬으로 넘어가는 로직 구현
        Debug.Log("Starting Battle with characters: " + battleCharacters.Count.ToString());
        GameUser.Instance.enemySamples = battleCharacters;
        GameUser.Instance.UpdateStatus();
        SceneManager.sceneLoaded += StartGame;
        SceneManager.LoadScene(battleScenePath, LoadSceneMode.Single);
    }

    public void EndBattle()
    {
        GameUser.Instance.UpdateStatus();
    }
    
    public void GoToWorldScene()
    {
        // 월드씬으로 넘어가는 로직 구현
        Debug.Log("Going to World Scene");
        SceneManager.LoadScene(WORLD_SCENE_INDEX, LoadSceneMode.Single);
    }
}
