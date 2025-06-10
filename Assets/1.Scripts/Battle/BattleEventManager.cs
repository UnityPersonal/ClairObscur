using System;
using UnityEngine;

public static class BattleEventManager 
{
    public class EventCallbacks
    {
        public Action<FocusEventArgs> OnFocus;
        public Action<AttackEventArgs> OnAttack;
        public Action<EvadeEventArgs> OnEvade;
        public Action<ParryEventArgs> OnParry;
        public Action<JumpEventArgs> OnJump;
        public Action<ShootEventArgs> OnShoot;
        public Action<TakeDamageEventArgs> OnTakeDamage;
    }

    private static EventCallbacks s_callbacks = new EventCallbacks();
    
    public static EventCallbacks Callbacks
    {
        get { return s_callbacks; }
    }
    
    static public void OnFocus(FocusEventArgs args)
    {
        s_callbacks.OnFocus?.Invoke(args);
    }
    
    static public void OnAttack(AttackEventArgs args)
    {
        s_callbacks.OnAttack?.Invoke(args);
    }
    
    static public void OnEvade(EvadeEventArgs args)
    {
        s_callbacks.OnEvade?.Invoke(args);
    }
    
    static public void OnParry(ParryEventArgs args)
    {
        s_callbacks.OnParry?.Invoke(args);
    }
    
    static public void OnJump(JumpEventArgs args)
    {
        s_callbacks.OnJump?.Invoke(args);
    }
    
    static public void OnShoot(ShootEventArgs args)
    {
        s_callbacks.OnShoot?.Invoke(args);
    }
}
