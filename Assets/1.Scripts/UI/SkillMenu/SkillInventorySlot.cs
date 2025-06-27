using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillInventorySlot : MonoBehaviour, 
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler,
    IPointerClickHandler , IPointerDownHandler
{
    public string SkillKey { get; private set; }
    public Image skillIcon;
    
    public Image apBackground;
    public TMP_Text apText;
    
    [SerializeField] private bool canDrag;
    public void SetSlot(SkillData slotItem, bool canDrag = true)
    {
        if (slotItem != null)
        {
            SkillKey = slotItem.Key;
            skillIcon.sprite = slotItem.SkillIcon;
            apText.text = slotItem.ApCost.ToString();
            this.canDrag = canDrag;
            
            skillIcon.color = new Color(1,1,1,canDrag ? 1 : 0.5f);
            
            skillIcon.enabled = true;
            apBackground.enabled = true;
            apText.enabled = true;
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        SkillKey = string.Empty;
        skillIcon.sprite = null;
        apText.text = string.Empty;
        this.canDrag = false;
            
        skillIcon.enabled = false;
        apText.enabled = false;
        apBackground.enabled = false;

    }
    
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (string.IsNullOrEmpty(SkillKey)) return;
        
        if (canDrag)
            CharacterSkillMenu.Instance.StartDrag(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (string.IsNullOrEmpty(SkillKey)) return;

        if (canDrag)
            CharacterSkillMenu.Instance.UpdatePosition(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (string.IsNullOrEmpty(SkillKey)) return;

        if (canDrag)
            CharacterSkillMenu.Instance.EndDrag(eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CharacterSkillMenu.Instance.PointerClick(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CharacterSkillMenu.Instance.PointerClick(this);
    }
}