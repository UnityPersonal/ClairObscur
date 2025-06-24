using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class SkillMenuSelectUI : MonoSingleton<SkillMenuSelectUI>
{
    public enum SelectType
    {
        MainMenu,
        Skill1,
        Skill2,
        Skill3
    }
    [SerializeField] private Button skillButton1;
    [SerializeField] private Button skillButton2;
    [SerializeField] private Button skillButton3;
    
    [SerializeField] private SkillButtonUI[] skillButtons;
    
    bool menuSelected = false;
    public bool MenuSelected => menuSelected;
    private BattlePlayer player;
    public int CurrentSelectIndex { get; set; } = 0;
    
    private void Awake()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            var skillButton = skillButtons[i];
            
            if (skillButton == null)
            {
                Debug.LogError("SkillMenuSelectUI ::: Awake - SkillButtonUI is null at index " + i);
                continue;
            }
            
            int selectIndex = i;
            skillButton.SkillButton.onClick.AddListener(() =>
            {
                CurrentSelectIndex = selectIndex;
                Debug.Log($"skillButtons[{selectIndex}] clicked");
                menuSelected = true;
            });
        }

    }

    public void UpdateSelectUI(BattlePlayer player)
    {
        this.player = player;
        menuSelected = false;
        
        for(int i = 0; i < skillButtons.Length; i++)
        {
            var skillData = player.GetSkillDataByIndex(i);
            skillButtons[i].SetUP(player,skillData);
        }
    }
    

}
