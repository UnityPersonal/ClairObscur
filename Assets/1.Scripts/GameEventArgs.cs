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
    public DodgeEventArgs(BattleCharacter character, float evadeTime) 
        : base(evadeTime)
    {
    }
}

public class ParryEventArgs : TimingEventArgs
{
    public BattleCharacter Character { get; private set; }
    public ParryEventArgs(BattleCharacter character, float parryTime)
        : base(parryTime)
    {
        Character = character;
    }
}

public class JumpEventArgs : TimingEventArgs
{
    
    public JumpEventArgs(BattleCharacter character, float jumpTime)
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

    public BattleAttackType AttackType { get; private set; }
    public float AttackTime { get; private set; } = 0f; // 공격이 시작된 시간
    public BattleCharacter Attacker { get; private set; }
    public BattleCharacter Target { get; private set; }
    
    public AttackEventArgs(
        int damage,
        float attackTime,
        BattleAttackType attackType,
        BattleCharacter attacker,
        BattleCharacter target
        )
    {
        Damage = damage;
        AttackTime = attackTime;
        AttackType = attackType;
        Attacker = attacker;
        Target = target;
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
        int damage
        )
    {
        Target = target;
        Damage = damage;
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
