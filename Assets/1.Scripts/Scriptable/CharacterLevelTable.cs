using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct CharacterGrowthCSVData
{
    public int Level { get; set; }
    public int Health { get; set; }
    public int AttackPower { get; set; }
    public int Speed { get; set; }
    public int Defense { get; set; }
    public int CriticalRate { get; set; }
    public int NextExp { get; set; }
}

public struct CharacterGrowthData
{
    public CharacterGrowthData(CharacterGrowthCSVData characterGrowthCSVData)
    {
        
        Exp = characterGrowthCSVData.NextExp;
        Level = characterGrowthCSVData.Level;
        Health = characterGrowthCSVData.Health;
        AttackPower = characterGrowthCSVData.AttackPower;
        Speed = characterGrowthCSVData.Speed;
        Defense = characterGrowthCSVData.Defense;
        CriticalRate = characterGrowthCSVData.CriticalRate;
        NextExp = characterGrowthCSVData.NextExp;
    }
    
    public int Level { get; set; }
    public int Health { get; set; }
    public int AttackPower { get; set; }
    public int Speed { get; set; }
    public int Defense { get; set; }
    public int CriticalRate { get; set; }
    public int NextExp { get; set; }
    public int Exp { get; set; } 
}

public class CharacterLevelTable : MonoBehaviour
{
    [SerializeField] string tablePath;
    private readonly List<CharacterGrowthData> characterGrowthDataList = new List<CharacterGrowthData>();

    private void Start()
    {
        //  load character growth data from CSV or other source
        TSVLoader.LoadTableAsync<CharacterGrowthCSVData>(tablePath, true).ContinueWith((taskResult) =>
        {
            var list = taskResult.Result;
            Debug.Log($"Loaded {list.Count} character growth data from {tablePath}");
            foreach (var csvData in list)
            {
                var growthData = new CharacterGrowthData(csvData);
                characterGrowthDataList.Add(growthData);
            }

        });
    }

    public CharacterGrowthData GetCharacterGrowthData(int level)
    {
        return characterGrowthDataList[level];
    }

}
