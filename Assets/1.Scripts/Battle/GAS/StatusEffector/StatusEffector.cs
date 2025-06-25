using System;
using UnityEngine;

public partial class StatusEffector
{
    public string EffectorName;
    public Sprite EffectorIcon;
    
    private int effectorValue = 0;

    public int EffectorValue
    {
        get { return effectorValue; }
        set
        {
            effectorValue = value;
            if (character != null)
            {
                // Trigger any callbacks or updates needed in the character
                character.Callbacks.OnEffectorChanged?.Invoke();
            }
        }
    }
    
    private BattleCharacter character;
    public void BindCharacter(BattleCharacter owner)
    {
        character = owner;
    }
    
    
    
}