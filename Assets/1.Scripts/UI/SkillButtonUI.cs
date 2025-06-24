using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonUI : MonoBehaviour
{
    [SerializeField] private Button skillButton;
    public Button SkillButton => skillButton;
    [SerializeField] private TMP_Text skillNameText;
    public TMP_Text SkillNameText => skillNameText;
    
    [SerializeField] private Image skillIcon;
    [SerializeField] private TMP_Text apCostText;
    [SerializeField] private TMP_Text descriptionText;
    public void SetUP(BattleCharacter character, SkillData skillData)
    {
        if (skillData == null)
        {
            this.skillIcon.sprite = null;
            this.skillNameText.text = "No Skill";
            this.apCostText.text = "0";
            this.descriptionText.text = "No description available.";
            return;
        }
        
        this.skillNameText.text = skillData.SkillName;
        this.skillIcon.sprite = skillData.SkillIcon;
        this.apCostText.text = skillData.ApCost.ToString();
        this.descriptionText.text = skillData.SkillDescription;
        var ap = character.Stat(GameStat.AP);
        if(ap.StatValue < skillData.ApCost )
        {
            skillButton.enabled = false;
            skillNameText.color = Color.red; // Disable color
        }
        else
        {
            skillButton.enabled = true;
            skillNameText.color = Color.black; // Enable color
        }
    }
}
