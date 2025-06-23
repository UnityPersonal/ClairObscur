using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSkillMenu : MonoBehaviour
{
    [SerializeField] Button exitButton;
    [SerializeField] SkillInventorySystem skillInventorySystem;
    
    [Header("Character Skill Detail Menu")]
    [SerializeField] TMP_Text skillNameText;
    [SerializeField] TMP_Text skillDescriptionText;
    [SerializeField] TMP_Text skillLearnCostText;
    [SerializeField] Button skillLearnButton;

    [Header("Character Skill Equip Menu")]
    [SerializeField] TMP_Text characterNameText;
    
    SkillDatabase skillDatabase;
    public void Setup(string characterName)
    {
        skillDatabase = WorldPlayer.player.GetSkillDatabase(characterName);
        exitButton.onClick.AddListener(()=>
        {
            gameObject.SetActive(false);
        });

        var status = GameUser.Instance.GetPlayerStatus(characterName);
        skillInventorySystem.SetUp(status, skillDatabase);
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
