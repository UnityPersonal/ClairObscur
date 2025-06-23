using System;
using System.Collections.Generic;
using UnityEngine;

public class GameUser : MonoBehaviour
{
    public static GameUser Instance { get; private set; }

    [SerializeField] public List<BattlePlayer>  playerSamples;
    [SerializeField] public List<BattleMonster> enemySamples;
    
    // 저장되어야할 유저 캐릭터 정보
    [SerializeField] PlayerStatus[] playerStatus;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 게임 오브젝트를 파괴하지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 중복 생성 방지
        }
    }
    
    public PlayerStatus GetPlayerStatus(string characterName)
    {
        foreach (var status in playerStatus)
        {
            if (status.CharacterName.Equals(characterName, StringComparison.OrdinalIgnoreCase))
            {
                return status;
            }
        }
        Debug.LogWarning($"Character '{characterName}' not found in GameUser data.");
        return null; // 캐릭터가 없을 경우 null 반환
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
