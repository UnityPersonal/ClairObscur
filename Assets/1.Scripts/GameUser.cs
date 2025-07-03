using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class GameUser : DontDestorySingleton<GameUser>
{
    [SerializeField] public List<BattlePlayer>  playerSamples;
    [SerializeField] public List<BattleMonster> enemySamples;
    
    // 저장되어야할 유저 캐릭터 정보
    [SerializeField] PlayerStatus[] playerStatus;

    protected override void Awake()
    {
        base.Awake();
    }

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

            const int AttributePointBonus = 5;
            var attributePoint = status.GetStat(CharacterStatus.ATTRIBUTE_POINT);
            
            CharacterLevelTable growthTable = AssetManager.Instance.GetCharacterAssetTable(status.CharacterName).characterLevelTable;
           
            { // update level up
                while (exp.StatValue >= nextExp.StatValue)
                {
                    exp.StatValue = (exp.StatValue - nextExp.StatValue);
                    level.IncrementStatValue(1);
                    var growthData = growthTable.GetCharacterGrowthData(level.StatValue);
                    nextExp.SetStatValue(growthData.NextExp);
                    
                    attributePoint.IncrementStatValue(AttributePointBonus);
                }
            }

            {// update stat with attributes
                var growthData = growthTable.GetCharacterGrowthData(level.StatValue);
                
                var vitalityAttr = status.GetAttribute("vitality");
                var agilityAttr = status.GetAttribute("agility");
                var mightAttr = status.GetAttribute("might");
                var defenseAttr = status.GetAttribute("defense");
                var luckAttr = status.GetAttribute("luck");
                
                int maxHP = growthData.Health + vitalityAttr.AttributeValue * 5;
                int power = growthData.AttackPower + mightAttr.AttributeValue * 5;
                int def = growthData.Defense + defenseAttr.AttributeValue * 2;
                int crit = growthData.CriticalRate + luckAttr.AttributeValue;
                int spd = growthData.Speed + agilityAttr.AttributeValue * 2;
                
                health.MaxValue = maxHP;
                health.SetStatValue(maxHP);
                attackPower.SetStatValue(power);
                defense.SetStatValue(def);
                critical.SetStatValue(crit);
                speed.SetStatValue(spd);
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
