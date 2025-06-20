using System;
using System.Collections.Generic;
using UnityEngine;

public enum MaelStanceType { None, Defense, Offense, Enlighten }

public abstract class MaelStanceState
{
    protected MaelPlayer player;

    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void OnBasicAttack() {}
    public virtual void OnTakeDamage(ref int damage) {}
    public virtual void OnHeal(int healAmount) {}
    public virtual void OnGiveDamage(ref int damage) {}

    public void Initialize(MaelPlayer player) => this.player = player;
}

public class DefenseStance : MaelStanceState
{
    public override void OnTakeDamage(ref int damage)
    {
        damage = Mathf.FloorToInt(damage * 0.75f); // 피해 감소
        var ap = player.Stat(GameStat.AP);
        ap.SetStatValue(ap.StatValue + 1);
    }
    public override void OnHeal(int healAmount)
    {
        var ap = player.Stat(GameStat.AP);
        ap.SetStatValue(ap.StatValue + 1);
    }
}

public class OffenseStance : MaelStanceState
{
    public override void OnGiveDamage(ref int damage)
    {
        damage = Mathf.FloorToInt(damage * 1.25f); // 공격 증가
    }
    public override void OnTakeDamage(ref int damage)
    {
        damage = Mathf.FloorToInt(damage * 1.25f); // 받는 피해 증가
    }
}

public class EnlightenStance : MaelStanceState
{
    public override void OnGiveDamage(ref int damage)
    {
        damage = Mathf.FloorToInt(damage * 1.5f); // 주는 피해 대폭 증가
    }
}

public class MaelPlayer : BattlePlayer
{
    private MaelStanceState currentStance;
    private Dictionary<MaelStanceType, MaelStanceState> stanceMap = new();
    public MaelStanceType CurrentStanceType { get; private set; } = MaelStanceType.None;

    public override void Initialize()
    {
        base.Initialize();
        stanceMap[MaelStanceType.Defense] = new DefenseStance();
        stanceMap[MaelStanceType.Offense] = new OffenseStance();
        stanceMap[MaelStanceType.Enlighten] = new EnlightenStance();

        foreach (var stance in stanceMap.Values)
            stance.Initialize(this);

        ChangeStance(MaelStanceType.Defense); // 초기 태세
    }

    public void ChangeStance(MaelStanceType newStance)
    {
        if (newStance == CurrentStanceType || newStance == MaelStanceType.None) return;

        currentStance?.Exit();
        currentStance = stanceMap[newStance];
        CurrentStanceType = newStance;
        currentStance.Enter();

        Debug.Log($"[MaelPlayer] 태세 전환: {newStance}");
    }

    public override void OnBeginDefendSignal()
    {
        base.OnBeginDefendSignal();
        ChangeStance(MaelStanceType.Defense);
    }

    public override void OnBeginAttackSignal()
    {
        ChangeStance(MaelStanceType.Offense);
        base.OnBeginAttackSignal();
    }

    protected override void OnTakedDamage(int damage)
    {
        currentStance?.OnTakeDamage(ref damage);
        base.OnTakedDamage(damage);
    }

    protected override int GetCurrentDamage()
    {
        int damage = base.GetCurrentDamage();
        currentStance?.OnGiveDamage(ref damage);
        return damage;
    }

    protected override void OnJumped()
    {
        base.OnJumped();
        if (CurrentStanceType == MaelStanceType.None)
        {
            var ap = Stat(GameStat.AP);
            ap.SetStatValue(ap.StatValue + 1);
        }
    }
}
