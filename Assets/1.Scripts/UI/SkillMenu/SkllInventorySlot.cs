using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkllInventorySlot : MonoBehaviour, 
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public SkillItem skillItem {get; set;}
    [FormerlySerializedAs("slotItem")] [SerializeField] private SkillSlotItem skillSlotItem;
    [SerializeField] private bool canDrag;
    public void SetSlot(SkillItem skillItem)
    {
        this.skillItem = skillItem;
        skillSlotItem.Set(skillItem);
    }

    public void ClearSlot()
    {
        skillSlotItem.Clear();
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