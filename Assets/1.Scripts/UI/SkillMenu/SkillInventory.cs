using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInventory : MonoBehaviour
{
    //보통 게임에선 이 슬롯을 최대값으로 적용시켜놓고 lock을 걸어버리는 방법을 사용
    private SkllInventorySlot[] slots;
    
    // Start is called before the first frame update
    public void Awake()
    {
        slots = GetComponentsInChildren<SkllInventorySlot>();
    }

    public void Initialize(List<SkillSlotItem> items)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            //임시 코드
            slots[i].SetSlot(null);
        }

        if (items == null) return;
        
        for (int i = 0; i < items.Count; i++)
        {
            if (slots.Length <= i - 1) break;
            slots[i].SetSlot(items[i]);
        }
    }

    public bool IsIn(SkllInventorySlot slot)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slot.Equals(slots[i])) return true;
        }

        return false;
    }
}