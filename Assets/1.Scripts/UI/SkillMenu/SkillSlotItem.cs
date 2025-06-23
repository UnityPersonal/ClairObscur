using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotItem : MonoBehaviour
{
    //슬롯에 표현될 정보량이 많아 질 경우 SlotItem에서 출력을 컨트롤 하는게
    //속이 편하다!
    
    [SerializeField] private Image myImage;
    [SerializeField] private Image image;
    
    // [SerializeField] private Image background;
    // [SerializeField] private Image icon;
    // [SerializeField] private TMP_Text countText;

    public void Set(SkillItem skillItem)
    {
        if(skillItem == null) SetActive(false);
        else
        {
            image.sprite = skillItem.Sprite;
            SetActive(true);
        }
    }

    public void Clear()
    {
        SetActive(false);
    }

    private void SetActive(bool active)
    {
        myImage.enabled = active;
        image.enabled = active;
        // background.enabled = active;
        // icon.enabled = active;
        // countText.enabled = active;
    }
   
}