using System;
using UnityEngine;

/// <summary>
/// 캐릭터가 플레이할 수 없게 합니다.
/// 대신 캐릭터가 자기 아군 중 하나를 공격합니다.
/// </summary>
public class ConfuseEffector : StatusEffector
{
    protected override void BindToEvents()
    {
        BattleEventManager.Callbacks.OnStartTurn += OnStartTurn;
    }

    protected override void OnStartTurn(StartTurnEventArgs args)
    {
        if (args.Character == owner)
        {
            Debug.Log($"[ConfuseEffector] {owner.name}가 아군을 공격합니다.");
            // owner.Attack(RandomAlly()); // 실제 아군 대상 공격 구현 필요
        }
    }
}