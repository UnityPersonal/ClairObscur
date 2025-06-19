using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class CharacterState
{
    public BattleCharacter character;

    public virtual void Initialize(BattleCharacter character)
    {
        this.character = character;
    }
    public abstract void Execute();
    public abstract void Enter();
    public abstract void Exit();

    public virtual void ReserveSwap(string nextState)
    {
    }
}