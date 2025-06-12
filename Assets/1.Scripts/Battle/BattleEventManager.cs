using System;
using UnityEngine;

public class BattleEventManager : MonoSingleton<BattleEventManager>
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

    private EventCallbacks s_callbacks = new EventCallbacks();
    
    public static EventCallbacks Callbacks
    {
        get { return Instance.s_callbacks; }
    }
    
    static public void OnTakeDamage(TakeDamageEventArgs args)
    {
        Instance.s_callbacks.OnTakeDamage?.Invoke(args);
    }
    
    static public void OnFocus(FocusEventArgs args)
    {
        Instance.s_callbacks.OnFocus?.Invoke(args);
    }
    
    static public void OnAttack(AttackEventArgs args)
    {
        Instance.s_callbacks.OnAttack?.Invoke(args);
    }
    
    static public void OnEvade(EvadeEventArgs args)
    {
        Instance.s_callbacks.OnEvade?.Invoke(args);
    }
    
    static public void OnParry(ParryEventArgs args)
    {
        Instance.s_callbacks.OnParry?.Invoke(args);
    }
    
    static public void OnJump(JumpEventArgs args)
    {
        Instance.s_callbacks.OnJump?.Invoke(args);
    }
    
    static public void OnShoot(ShootEventArgs args)
    {
        Instance.s_callbacks.OnShoot?.Invoke(args);
    }
}
