using UnityEngine;

public class HitUIManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    
    [SerializeField] HitUI hitUIPrefab;
    
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
    }

    private void OnDodged(DodgeEventArgs args)
    {
        var hitUI = Instantiate(hitUIPrefab, canvas.transform);
        hitUI.Set( canvas, args.Character.transform, "DODGED");
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
    }
}
