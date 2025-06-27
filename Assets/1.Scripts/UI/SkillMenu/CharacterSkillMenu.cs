using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSkillMenu : MonoSingleton<CharacterSkillMenu>
{
    [SerializeField] Button exitButton;
    
    [Header("Character Skill Detail Menu")]
    [SerializeField] TMP_Text skillNameText;
    [SerializeField] TMP_Text skillDescriptionText;
    [SerializeField] TMP_Text skillLearnCostText;
    [SerializeField] Button skillLearnButton;

    [Header("Character Skill Equip Menu")]
    [SerializeField] TMP_Text characterNameText;
    
    [SerializeField] private SkillInventory equipmentSkillInventory;
    [SerializeField] private SkillInventory playerSkillInventory;

    [SerializeField] GraphicRaycaster raycaster;
    private SkillInventorySlot SourceSlot { get; set; }
    [SerializeField] private SkillInventorySlot dragSlot;

    public PlayerStatus currentStatus {get; private set;}
    public SkillDatabase currentDatabase { get; private set; }
    
    SkillDatabase skillDatabase;
    public void Setup(string characterName)
    {
        characterNameText.text = characterName;
        currentDatabase = AssetManager.Instance.GetCharacterAssetTable(characterName).skillDatabase;
        exitButton.onClick.AddListener(()=>
        {
            gameObject.SetActive(false);
        });

        currentStatus = GameUser.Instance.GetPlayerStatus(characterName);
        
        {
            var equipmentSkills = currentStatus.EquippedSkills.ToList();
            Debug.Log($"Equipment Skills Count: {equipmentSkills.Count}");
            equipmentSkillInventory.Initialize(equipmentSkills);
        }

        {

            var playerSkills = currentDatabase.skillTable.Values.ToList();
            Debug.Log($"Player Skills Count: {playerSkills.Count}");
            playerSkillInventory.Initialize(playerSkills);
        }
    }
    
    public void StartDrag(SkillInventorySlot source)
    {
        Debug.Log($"StartDrag {source.SkillKey}");
        // source를 캐싱을 해준다 
        SourceSlot = source;
        var skill =currentDatabase.GetSkillData(source.SkillKey);
        dragSlot.gameObject.SetActive(true);
        dragSlot.SetSlot(skill);
    }

    //드래그 중일때 드래그 중인 슬롯의 위치를 갱신하기 위한 함수 
    public void UpdatePosition(Vector2 position)
    {
        dragSlot.transform.position = position;
    }

    //Drag가 끝날때의 이벤트 데이터를 사용해서 어느 인벤토리인지, 어느슬롯인지 판별
    public void EndDrag(PointerEventData eventData)
    {
        Debug.Log($"EndDrag {SourceSlot.SkillKey}");
        dragSlot.ClearSlot();
        dragSlot.gameObject.SetActive(false);
        var results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        for (int i = 0; i < results.Count; i++)
        {
            Debug.Log($"Ray Result {results[i].gameObject.name}");
            SkillInventorySlot targetSlot = 
                results[i].gameObject.GetComponent<SkillInventorySlot>();
            if (targetSlot != null && 
                targetSlot != SourceSlot && targetSlot != dragSlot)
            {
                Debug.Log($"FindInventory {targetSlot.SkillKey}");
                SkillInventory from = FindInventory(SourceSlot);
                SkillInventory to = FindInventory(targetSlot);

                if (from.Equals(to))
                {
                    //아무일도 일어나지 않음.
                }
                else
                {
                    if (from.Equals(playerSkillInventory))
                    {
                        // 플레이어 인벤토리에서 장착된 스킬을 제거
                        ApplyItem(SourceSlot, targetSlot);
                    }
                }
                
            }
        }
        
    }

    private void ApplyItem(SkillInventorySlot source, SkillInventorySlot target)
    {
        var skill = currentDatabase.GetSkillData(source.SkillKey);
        target.SetSlot(skill, false);
        // todo: target에 아이템을 적용하는 로직을 작성해야함

        var slots = equipmentSkillInventory.Slots;
        int equipIndex = 0;
        for (; equipIndex < slots.Length; equipIndex++)
        {
            if(slots[equipIndex].SkillKey.Equals(skill.Key, StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
        }
        Debug.Log($"스킬 장착 {equipIndex} {target.gameObject.name}");

        currentStatus.EquippedSkills[equipIndex] = skill;
    }

    private SkillInventory FindInventory(SkillInventorySlot slot)
    {
        return equipmentSkillInventory.IsIn(slot) ? equipmentSkillInventory : playerSkillInventory;
    }
    
    public void PointerClick(SkillInventorySlot slot)
    {
        var skill = currentDatabase.GetSkillData(slot.SkillKey);
        
        skillNameText.text = skill.SkillName;
        skillDescriptionText.text = skill.SkillDescription;
        skillLearnCostText.text = skill.LearnCost.ToString();

        skillLearnButton.gameObject.SetActive(currentStatus.LearnedSkills.Contains(skill) == false);
    }
    
}
