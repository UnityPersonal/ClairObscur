using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonUI : MonoBehaviour
{
    [SerializeField] private Button skillButton;
    public Button SkillButton => skillButton;
    [SerializeField] private TMP_Text skillNameText;
    public TMP_Text SkillNameText => skillNameText;

    public void SetUP(BattleCharacter character, SkillData skillData)
    {
        this.skillNameText.text = skillData.skillName;
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
