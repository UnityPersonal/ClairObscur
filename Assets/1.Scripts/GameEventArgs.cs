using UnityEngine;

public class GameEventArgs
{
}

public class TimingEventArgs : GameEventArgs
{
    public float Time { get; private set; }
    
    public TimingEventArgs(float time)
    {
        Time = time;
    }
}

public class DodgeEventArgs : TimingEventArgs
{
    public DodgeEventArgs(float evadeTime) 
        : base(evadeTime)
    {
    }
}

public class ParryEventArgs : TimingEventArgs
{
    public ParryEventArgs(float parryTime)
        : base(parryTime)
    {
    }
}

public class JumpEventArgs : TimingEventArgs
{
    
    public JumpEventArgs(float jumpTime)
        : base(jumpTime)
    {
    }
}


public class ShootEventArgs : GameEventArgs
{
    public Vector3 HitPosition { get; private set; }
    public BattleCharacter Target { get; private set; }
    
    public ShootEventArgs(Vector3 hitPosition, BattleCharacter target)
    {
        HitPosition = hitPosition;
        Target = target;
    }
}

public class FocusEventArgs : GameEventArgs
{
    public BattleCharacter Target { get; private set; }
    
    public FocusEventArgs(BattleCharacter target)
    {
        Target = target;
    }
}

public class AttackEventArgs : GameEventArgs
{
    public int Damage { get; private set; }
    
    public BattleCharacter Attacker { get; private set; }
    public BattleCharacter Target { get; private set; }
    
    public bool Dodged { get; set; } = false;
    public bool Parried { get; set; } = false;
    public bool Jumped { get; set; } = false;
    
    public AttackEventArgs(
        int damage,
        BattleCharacter attacker,
        BattleCharacter target,
        bool dodged = false,
        bool parried = false,
        bool jumped = false
        )
    {
        Damage = damage;
        Attacker = attacker;
        Target = target;
        Dodged = dodged;
        Parried = parried;
        Jumped = jumped;
    }
}

public class TakeDamageEventArgs : GameEventArgs
{
    public BattleCharacter Target { get; private set; }
    public int Damage { get; private set; }
    public bool Dodged { get; set; } = false;
    public bool Parried { get; set; } = false;
    public bool Jumped { get; set; } = false;
    
    public TakeDamageEventArgs(
        BattleCharacter target,
        int damage,
        bool dodged,
        bool parried,
        bool jumped)
    {
        Target = target;
        Damage = damage;
        Dodged = dodged;
        Parried = parried;
        Jumped = jumped;
    }
}

public class DeathEventArgs : GameEventArgs
{
    public BattleCharacter Target { get; private set; }
    
    public DeathEventArgs(BattleCharacter target)
    {
        Target = target;
    }
}
