using UnityEngine;

/// <summary>
/// 캐릭터가 받는 다음 피해를 50% 증가시킵니다.
/// 징표는 한 번의 피해에만 영향을 줍니다.
/// </summary>
public class MarkEffector : StatusEffector
{
    public override string EffectorName => "Mark";
    private bool isMarked = true;

    protected override void BindToEvents()
    {
        BattleEventManager.Callbacks.OnTakeDamage += OnTakeDamage;
    }

    protected override void OnTakeDamage(TakeDamageEventArgs args)
    {
        if (isMarked && args.Target == owner)
        {
            int bonus = Mathf.CeilToInt(args.Damage * 0.5f);
            Debug.Log($"[MarkEffector] {owner.name} 추가 피해 {bonus}");
            owner.Status.CurrentHP -= bonus;
            isMarked = false;
        }
    }
}