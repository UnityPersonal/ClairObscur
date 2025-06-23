using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkllInventorySlot : MonoBehaviour, 
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public SkillSlotItem skillItem {get; set;}
    [SerializeField] private bool canDrag;
    public void SetSlot(SkillSlotItem skillItem)
    {
        this.skillItem = skillItem;
    }

    public void ClearSlot()
    {
        skillItem = null;
    }
    
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (skillItem == null) return;
        if (canDrag)
            SkillInventorySystem.Instance.StartDrag(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SkillInventorySystem.Instance.UpdatePosition(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SkillInventorySystem.Instance.EndDrag(eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        
    }
}