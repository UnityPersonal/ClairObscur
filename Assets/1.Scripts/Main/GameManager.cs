using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : DontDestorySingleton<GameManager>
{
    const int LOBBY_SCENE_INDEX = 0;
    const int WORLD_SCENE_INDEX = 1;
    const int BATTLE_SCENE_INDEX = 2;
    
    public void StartBattle(List<BattleCharacter> battleCharacters)
    {
        // 배틀씬으로 넘어가는 로직 구현
        Debug.Log("Starting Battle with characters: " + battleCharacters.Count.ToString());
        SceneManager.LoadScene(BATTLE_SCENE_INDEX, LoadSceneMode.Single);
    }
    
    public void GoToWorldScene()
    {
        // 월드씬으로 넘어가는 로직 구현
        Debug.Log("Going to World Scene");
        SceneManager.LoadScene(WORLD_SCENE_INDEX, LoadSceneMode.Single);
    }
}
