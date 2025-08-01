using System;
using UnityEngine;

public class BattleEventManager : MonoSingleton<BattleEventManager>
{
    public class EventCallbacks
    {
        public Action<StartTurnEventArgs> OnStartTurn;
        public Action<EndTurnEventArgs> OnEndTurn;
        public Action<FocusEventArgs> OnFocus;
        public Action<AttackEventArgs> OnAttack;
        public Action<DodgeEventArgs> OnDodge;
        public Action<ParryEventArgs> OnParry;
        public Action<JumpEventArgs> OnJump;
        public Action<ShootEventArgs> OnShoot;
        public Action<TakeDamageEventArgs> OnTakeDamage;
        public Action<DeathEventArgs> OnDeath;
        public Action<CounterEventArgs> OnCounter;
        
        public Action<StartDefenseEventArgs> OnStartDefend;
        public Action<EndDefenseEventArgs> OnEndDefend;
        
    }

    private EventCallbacks callbacks = new EventCallbacks();
    
    public static EventCallbacks Callbacks
    {

        get { return Instance.callbacks; }
    }
    
    static public void OnTakeDamage(TakeDamageEventArgs args)
    {
        Instance.callbacks.OnTakeDamage?.Invoke(args);
    }
    
    static public void OnFocus(FocusEventArgs args)
    {
        Instance.callbacks.OnFocus?.Invoke(args);
    }
    
    static public void OnAttack(AttackEventArgs args)
    {
        Instance.callbacks.OnAttack?.Invoke(args);
    }
    
    static public void OnDodge(DodgeEventArgs args)
    {
        Instance.callbacks.OnDodge?.Invoke(args);
    }
    
    static public void OnParry(ParryEventArgs args)
    {
        Instance.callbacks.OnParry?.Invoke(args);
    }
    
    static public void OnJump(JumpEventArgs args)
    {
        Instance.callbacks.OnJump?.Invoke(args);
    }
    
    static public void OnShoot(ShootEventArgs args)
    {
        Instance.callbacks.OnShoot?.Invoke(args);
    }
    
    static public void OnDeath(DeathEventArgs args)
    {
        Instance.callbacks.OnDeath?.Invoke(args);
    }
    
    static public void OnCounter(CounterEventArgs args)
    {
        Instance.callbacks.OnCounter?.Invoke(args);
    }

    static public void OnStartTurn(StartTurnEventArgs args)
    {
        Instance.callbacks.OnStartTurn?.Invoke(args);
    }
    
    static public void OnEndTurn(EndTurnEventArgs args)
    {
        Instance.callbacks.OnEndTurn?.Invoke(args);
    }
    
    static public void OnStartDefend(StartDefenseEventArgs args)
    {
        Instance.callbacks.OnStartDefend?.Invoke(args);
    }

    static public void OnEndDefend(EndDefenseEventArgs args)
    {
        Instance.callbacks.OnEndDefend?.Invoke(args);
    }
}


