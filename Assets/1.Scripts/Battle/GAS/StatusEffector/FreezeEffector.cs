using UnityEngine;

/// <summary>
/// 캐릭터가 여러 턴 동안 연속으로 플레이할 수 없게 합니다.
/// 캐릭터가 피해를 받으면 빙결 효과가 사라집니다.
/// </summary>
public class FreezeEffector : StatusEffector
{
    public override string EffectorName => "freeze";
    protected override void BindToEvents()
    {
        BattleEventManager.Callbacks.OnStartTurn += OnStartTurn;
    }

    protected override void OnStartTurn(StartTurnEventArgs args)
    {
        if (args.Character == owner)
        {
            Debug.Log($"[FreezeEffector] {owner.name}의 턴을 스킵합니다.");
            owner.Deactivate();
        }
    }
}