using System;
using UnityEngine;

public class HitUIManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    
    [SerializeField] HitUI hitUIPrefab;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip dodgeSound;
    [SerializeField] AudioClip parrySound;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BattleEventManager.Callbacks.OnTakeDamage += OnTakeDamage;
        BattleEventManager.Callbacks.OnDodge += OnDodged;
        BattleEventManager.Callbacks.OnParry += OnParried;
        BattleEventManager.Callbacks.OnJump += OnJumped;
        
    }

    private void OnParried(ParryEventArgs args)
    {
        var hitUI = Instantiate(hitUIPrefab, canvas.transform);
        hitUI.Set( canvas, args.Character.transform, "PARRIED");
        hitUI.audioSource.clip = parrySound; 
        
    }

    private void OnDodged(DodgeEventArgs args)
    {
        var hitUI = Instantiate(hitUIPrefab, canvas.transform);
        hitUI.Set( canvas, args.Character.transform, "DODGED");
        hitUI.audioSource.clip = dodgeSound;
    }
    
    private void OnJumped(JumpEventArgs args)
    {
        var hitUI = Instantiate(hitUIPrefab, canvas.transform);
        hitUI.Set( canvas, args.Character.transform, "JUMPED");
    }

    void OnTakeDamage(TakeDamageEventArgs args)
    {
        var hitUI = Instantiate(hitUIPrefab, canvas.transform);
        string damageText = args.Damage.ToString();
        hitUI.Set( canvas, args.Target.transform, damageText);
        hitUI.audioSource.clip = hitSound;
    }
}
