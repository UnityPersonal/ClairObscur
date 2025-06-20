using System;
using UnityEngine;

/// <summary>
/// 상태효과의 기본 클래스입니다. 각 Effector는 이 클래스를 상속받고,
/// 필요한 타이밍 이벤트에 콜백을 등록합니다.
/// </summary>
[Serializable]
public abstract class StatusEffector
{
    public abstract string EffectorName { get; }
    protected BattleCharacter owner;

    public virtual void Initialize(BattleCharacter owner)
    {
        this.owner = owner;
        BindToEvents();
    }

    /// <summary>
    /// 필요한 타이밍에 따라 BattleEventManager 이벤트에 콜백을 등록합니다.
    /// </summary>
    protected virtual void BindToEvents() { }

    protected virtual void OnStartTurn(StartTurnEventArgs args) { }
    protected virtual void OnTakeDamage(TakeDamageEventArgs args) { }
    protected virtual void OnDeath(DeathEventArgs args) { }
}