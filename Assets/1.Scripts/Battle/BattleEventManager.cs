using System;
using UnityEngine;

public class BattleEventManager : MonoSingleton<BattleEventManager>
{
    public class EventCallbacks
    {
        public Action<FocusEventArgs> OnFocus;
        public Action<AttackEventArgs> OnAttack;
        public Action<DodgeEventArgs> OnDodge;
        public Action<ParryEventArgs> OnParry;
        public Action<JumpEventArgs> OnJump;
        public Action<ShootEventArgs> OnShoot;
        public Action<TakeDamageEventArgs> OnTakeDamage;
        public Action<DeathEventArgs> OnDeath;
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
}
