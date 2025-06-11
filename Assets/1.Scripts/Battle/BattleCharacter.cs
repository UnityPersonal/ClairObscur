using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public enum BattleCharacterType
{
    Player,
    Enemy
}

public enum BattleActionType
{
    Relax,
    Attack,
    Defend
}

public abstract class BattleCharacter : MonoBehaviour
{
    [Header("Character Location Settings")]
    [SerializeField] private Transform characterDefaultLocation;
    
    [Space(10), Header("Character Camera Settings")]
    [SerializeField] private CinemachineCamera characterCamera;
    
    [Space(10), Header("Character Timeline Settings")]
    [SerializeField] protected PlayableDirector director;
    
    abstract public  BattleCharacterType CharacterType { get; }

    [Space(10), Header("Character Status")]
    [SerializeField] private int maxHp = 100;
    private int currentHp = 100;
    public int CurrentHp 
    {
        get { return currentHp; }
        protected set 
        {
            currentHp = Mathf.Clamp(value, 0, maxHp);
        }
    }
    
    public bool IsDead 
    {
        get { return currentHp <= 0; }
    }

    protected bool IsAttacking = false;
    
    // Update is called once per frame
    void Update()
    {
        if(Activated == false)
        {
            return;
        }

        if (IsDead == true)
        {
            return;
        }
        
        // Logic for character actions during the turn
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
    }
    
    public void OnFocused(BattleCharacter focusedCharacter)
    {
        Debug.Log($"BattleCharacter ::: OnFocused: {focusedCharacter.name}");
        // Logic to handle when this character is focused, e.g., highlighting the character
    }

    bool _activated = false;
    public bool Activated
    {
        get
        {
            return _activated;
        }
        protected set
        {
            if (value == true)
            {
                // 캐릭터가 활성화 되면 카메라 위선순위를 높혀 캐릭터에 포커싱 되도록한다.
                characterCamera.Priority = 20;
            }
            else
            {
                // 캐릭터가 비활성화 되면(턴 종료) 우선순위를 낮춰 자동으로 포커스 아웃되도록 처리한다.
                characterCamera.Priority = 0;
            }
            _activated = value;
        }
        
    }
    public void StartTurn()
    {
        Debug.Log($"BattleCharacter ::: StartTurn {name}");
        Activated = true;
        StartCoroutine(UpdateBattleActionCoroutine());
    }

    protected abstract IEnumerator UpdateBattleActionCoroutine();
}
