using System;
using System.Collections;
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
    private SelectType currentSelectType = SelectType.Skill1;
    public SelectType CurrentSelectType => currentSelectType;
    
    [SerializeField] private Button skillButton1;
    [SerializeField] private Button skillButton2;
    [SerializeField] private Button skillButton3;
    bool menuSelected = false;
    private BattlePlayer player;
    
    private void Awake()
    {
        skillButton1.onClick.AddListener(() =>
        {
            currentSelectType = SelectType.Skill1;
            menuSelected = true;
        });
        skillButton2.onClick.AddListener(() =>
        {
            currentSelectType = SelectType.Skill2;
            menuSelected = true;
        });
        skillButton3.onClick.AddListener(() =>
        {
            currentSelectType = SelectType.Skill3;
            menuSelected = true;
        });
    }

    public IEnumerator UpdateSelectUI(BattlePlayer player)
    {
        this.player = player;
        menuSelected = false;
        while (!menuSelected)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                currentSelectType = SelectType.MainMenu;
                menuSelected = true;
            }
            yield return null;
        }
    }
    

}
