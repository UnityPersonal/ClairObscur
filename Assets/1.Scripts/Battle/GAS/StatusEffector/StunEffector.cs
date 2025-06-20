using UnityEngine;

/// <summary>
/// 캐릭터가 플레이할 수 없게 합니다.
/// </summary>
public class StunEffector : StatusEffector
{
    protected override void BindToEvents()
    {
        BattleEventManager.Callbacks.OnStartTurn += OnStartTurn;
    }

    protected override void OnStartTurn(StartTurnEventArgs args)
    {
        if (args.Character == owner)
        {
            Debug.Log($"[StunEffector] {owner.name} 기절로 인해 행동 불가");
            owner.Deactivate();
        }
    }
}