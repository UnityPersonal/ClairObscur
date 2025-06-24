using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MainMenulSelectUI : MonoSingleton<MainMenulSelectUI>
{
    [SerializeField] private Button attackButton;
    [SerializeField] private Button skillButton;
    [SerializeField] private Button itemButton;
    
    public enum SelectType
    {
        Attack,
        Skill,
        Item
    } 
    private SelectType currentSelectType = SelectType.Attack;
    public SelectType CurrentSelectType => currentSelectType;
    
    private BattlePlayer player;
    bool menuSelected = false;
    public bool MenuSelected => menuSelected;

    private void Awake()
    {
        attackButton.onClick.AddListener(() =>
        {
            currentSelectType = SelectType.Attack;
            menuSelected = true; 
        });
        skillButton.onClick.AddListener(() =>
        {
            currentSelectType = SelectType.Skill;
            menuSelected = true;
        });
        itemButton.onClick.AddListener(() =>
        {
            currentSelectType = SelectType.Item;
            //menuSelected = true;
        });
    }

    private void OnEnable()
    {
        menuSelected = false;
    }

    public void UpdateSelectUI(BattlePlayer player)
    {
        this.player = player;

        if (player.StatusEffect("silence").EffectorValue > 0)
        {
            // If the player is silenced, disable the skill button
            skillButton.enabled = false;
        }
        else
        {
            skillButton.enabled = true;
        }
    }
    

}
