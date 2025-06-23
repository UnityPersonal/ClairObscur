using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillInventorySystem : MonoBehaviour
{
    #region 이렇게 하시면 안됩니다~ 저는 스크립트 늘리기 싫어서 그냥 하는거에요

    private List<SkillItem> itemTable;

    private List<SkillItem> userDataItems = new List<SkillItem>();
    
    #endregion
    
    public static SkillInventorySystem Instance { get; private set; }

    private SkllInventorySlot SourceSlot { get; set; }
    [SerializeField] private SkllInventorySlot dragSlot;

    private GraphicRaycaster raycaster;

    [FormerlySerializedAs("traderInventory")] [SerializeField] private SkillInventory traderSkillInventory;
    [FormerlySerializedAs("userInventory")] [SerializeField] private SkillInventory userSkillInventory;
    
    
    
    private void Awake()
    {
        Instance = this;
        raycaster = GetComponent<GraphicRaycaster>();
    }

    public void SetUp(SkillDatabase database)
    {
        traderSkillInventory.Initialize(itemTable);
        userSkillInventory.Initialize(null);

    }

    public void StartDrag(SkllInventorySlot source)
    {
        // source를 캐싱을 해준다 
        SourceSlot = source;
        dragSlot.SetSlot(SourceSlot.skillItem);
    }

    //드래그 중일때 드래그 중인 슬롯의 위치를 갱신하기 위한 함수 
    public void UpdatePosition(Vector2 position)
    {
        dragSlot.transform.position = position;
    }

    //Drag가 끝날때의 이벤트 데이터를 사용해서 어느 인벤토리인지, 어느슬롯인지 판별
    public void EndDrag(PointerEventData eventData)
    {
        dragSlot.ClearSlot();
        
        var results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        for (int i = 0; i < results.Count; i++)
        {
            SkllInventorySlot targetSlot = 
                results[i].gameObject.GetComponent<SkllInventorySlot>();
            if (targetSlot != null && 
                targetSlot != SourceSlot && targetSlot != dragSlot)
            {
                SkillInventory from = FindInventory(SourceSlot);
                SkillInventory to = FindInventory(targetSlot);

                if (from.Equals(to))
                {
                    //아무일도 일어나지 않음.
                }
                else
                {
                    if (from.Equals(traderSkillInventory))
                    {
                        //Trader -> User (구매)
                        Debug.Log($"아이템 구매 {SourceSlot.gameObject.name}");
                    }
                    else
                    {
                        //User -> Trader (판매)
                        Debug.Log($"아이템 판매 {SourceSlot.gameObject.name}");
                    }
                }
                
                SwapItem(SourceSlot, targetSlot);
            }
        }
    }

    private void SwapItem(SkllInventorySlot a, SkllInventorySlot b)
    {
        var temp = a.skillItem;
        a.SetSlot(b.skillItem);
        b.SetSlot(temp);
    }

    private SkillInventory FindInventory(SkllInventorySlot slot)
    {
        return traderSkillInventory.IsIn(slot) ? traderSkillInventory : userSkillInventory;
    }
}