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
        string damageText = args.Damage.ToString();
        if (args.Parried)
        {
            damageText = "PARRIED";
        }
        else if (args.Dodged)
        {
            damageText = "DODGED";
        }
        else if (args.Jumped)
        {
            damageText = "JUMPED";
        }
        hitUI.Set( canvas, args.Target.transform, damageText);
    }
}
