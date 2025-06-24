using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class SkillDatabase : MonoBehaviour
{
    [SerializeField] string databasePath = "Database/Skills";
    
    [SerializeField] Sprite[] iconList; // 기본 스킬 아이콘, 필요시 변경 가능
    public ActionDataTable actionTable; // 액션 데이터, 필요시 변경 가능
    public Dictionary<string,SkillData> skillTable = new(); 

    public Dictionary<string, Sprite> iconTable = new();
        
    private void Start()
    {
        foreach (var icon in iconList)
        {
            iconTable[icon.name] = icon; 
        }
        
        
        TSVLoader.LoadTableAsync<VersoSkillCSVData>(databasePath, true).ContinueWith(
            (taskResult) =>
            {
                var list = taskResult.Result;
                Debug.Log($"loaded {list.Count} Verso skills from {databasePath}");
                foreach (var csvData in list)
                {
                    Debug.Log(csvData);
                    var skill = new VersoSkillData(csvData,this);
                    skillTable[skill.Key] = skill;
                }
            }
        );
        
        return;
    }

    
    public SkillData GetSkillData(string skillName)
    {
        return skillTable[skillName];
    }
}
