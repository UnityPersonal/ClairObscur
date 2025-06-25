using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class GameUser : MonoSingleton<GameUser>
{
    [SerializeField] public List<BattlePlayer>  playerSamples;
    [SerializeField] public List<BattleMonster> enemySamples;
    
    // 저장되어야할 유저 캐릭터 정보
    [SerializeField] PlayerStatus[] playerStatus;

    public void UpdateStatus()
    {
        foreach (var status in playerStatus)
        {
            var level = status.GetStat(CharacterStatus.LEVEL); // 레벨 초기화
            var exp =  status.GetStat(CharacterStatus.EXP); // 경험치 초기화
            var nextExp = status.GetStat(CharacterStatus.NEXT_EXP); // 다음 레벨 경험치
            var health = status.GetStat(CharacterStatus.HEALTH); // 체력
            var attackPower = status.GetStat(CharacterStatus.ATTACK_POWER); // 공격력
            var defense = status.GetStat(CharacterStatus.DEFENSE); // 방어력
            var critical = status.GetStat(CharacterStatus.CRITICAL_RATE);
            var speed = status.GetStat(CharacterStatus.SPEED); // 속도

            CharacterLevelTable growthTable = AssetManager.Instance.GetCharacterAssetTable(status.CharacterName).characterLevelTable;
            while (exp.StatValue >= nextExp.StatValue)
            {
                exp.StatValue = (exp.StatValue - nextExp.StatValue);
                level.IncrementStatValue(1);
                var growthData = growthTable.GetCharacterGrowthData(level.StatValue);
                nextExp.SetStatValue(growthData.NextExp);
                
                health.MaxValue = growthData.Health;
                health.SetStatValue(growthData.Health);
                
                attackPower.SetStatValue(growthData.AttackPower);
                defense.SetStatValue(growthData.Defense);
                critical.SetStatValue(growthData.CriticalRate);
                speed.SetStatValue(growthData.Speed);
            }
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
   
}
