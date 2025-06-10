using System;
using System.Collections;
using UnityEngine;

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
    [Header("Character Team Settings")]
    [SerializeField] private BattleCharacterType characterType = BattleCharacterType.Player;
    public BattleCharacterType CharacterType
    {
        get { return characterType; }
    }

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
    
    public bool Activated { get; protected set; } = false;
    public void Activate()
    {
        Debug.Log("BattleCharacter ::: Activate");
        Activated = true;
        
        StartCoroutine(UpdateBattleActionCoroutine());
    }
    
    public void StartTurn()
    {
        Debug.Log("BattleCharacter ::: StartTurn");
        // Logic to start the turn, e.g., enabling player actions
        Activated = true;
    }

    protected abstract IEnumerator UpdateBattleActionCoroutine();
}
