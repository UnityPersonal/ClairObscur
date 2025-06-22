using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillDatabase : MonoSingleton<SkillDatabase>
{
    [SerializeField] SkillData[] skills;
    [SerializeField] string databasePath = "Database/Skills";
    
    Dictionary<string,VersoSkillData> versoSkillDataDictionary 
        = new Dictionary<string, VersoSkillData>();
        

    
    private void Start()
    {
        TSVLoader.LoadTableAsync<VersoSkillCSVData>("DialogTable", true).ContinueWith(
            (taskResult) =>
            {
                var list = taskResult.Result;
                foreach (var csvData in list)
                {
                    var skill = new VersoSkillData(csvData);
                    versoSkillDataDictionary[skill.SkillName] = skill;
                }
            }
        );
    }

    
    public SkillData GetSkillData(string skillName)
    {
        foreach (var skill in skills)
        {
            if (skill.SkillName == skillName)
            {
                return skill;
            }
        }
        return null; // or throw an exception if not found
    }
}
