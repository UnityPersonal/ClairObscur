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
            menuSelected = true;
        });
    }
    
    public IEnumerator UpdateSelectUI(BattlePlayer player)
    {
        this.player = player;
        menuSelected = false;
        yield return new WaitUntil(() => menuSelected);
    }
    

}
