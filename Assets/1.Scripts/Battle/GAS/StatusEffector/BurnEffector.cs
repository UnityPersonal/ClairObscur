using UnityEngine;

/// <summary>
/// 캐릭터의 턴이 시작될 때 피해를 줍니다.
/// 다수의 연소 효과가 부여될 수 있으며, 이 경우 받는 연소 피해가 증가합니다.
/// </summary>
public class BurnEffector : StatusEffector
{
    protected override void BindToEvents()
    {
        BattleEventManager.Callbacks.OnStartTurn += OnStartTurn;
    }

    protected override void OnStartTurn(StartTurnEventArgs args)
    {
        if (args.Character == owner)
        {
            Debug.Log($"[BurnEffector] {owner.name}에게 연소 피해 적용");
            owner.Status.CurrentHP -= 5;
        }
    }
}