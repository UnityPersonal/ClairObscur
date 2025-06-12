using UnityEngine;

public class HitUIManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    
    [SerializeField] HitUI hitUIPrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BattleEventManager.Callbacks.OnTakeDamage += OnTakeDamage;
    }

    void OnTakeDamage(TakeDamageEventArgs args)
    {
        var hitUI = Instantiate(hitUIPrefab, canvas.transform);
        hitUI.Set( canvas, args.Target.transform, args.Damage);
    }
}
