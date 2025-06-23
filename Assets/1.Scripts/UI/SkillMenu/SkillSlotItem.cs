using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlotItem : MonoBehaviour , IPointerClickHandler
{
    //슬롯에 표현될 정보량이 많아 질 경우 SlotItem에서 출력을 컨트롤 하는게
    //속이 편하다!
    
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private TMP_Text SkillAp;
    
    // [SerializeField] private Image background;
    // [SerializeField] private Image icon;
    // [SerializeField] private TMP_Text countText;

    public void Set(SkillSlotItem skillSlotItem)
    {
        if (skillSlotItem == null)
        {
            skillIcon.sprite = null;
            SkillAp.text = "0";
            SetActive(false);
            return;
        }
        
        skillIcon.sprite = skillSlotItem.skillIcon.sprite;
        SkillAp.text = skillSlotItem.SkillAp.text;
        SetActive(true);
    }

    public void Clear()
    {
        SetActive(false);
    }

    private void SetActive(bool active)
    {
        skillIcon.enabled = active;
        SkillAp.enabled = active;
        // background.enabled = active;
        // icon.enabled = active;
        // countText.enabled = active;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}