using System;
using System.Collections.Generic;
using UnityEngine;

public enum MaelStanceType { None, Defense, Offense, Virtusoe }

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

public class StancelessStance : MaelStanceState
{
    public override void Enter()
    {
        var ap = player.Stat(GameStat.AP);
        ap.SetStatValue(ap.StatValue + 1); // 기본 AP 증가
    }

    public override void OnBasicAttack()
    {
        var ap = player.Stat(GameStat.AP);
        ap.SetStatValue(ap.StatValue + 1); // 기본 공격시 AP 증가
    }

    public override void OnTakeDamage(ref int damage)
    {
        damage = Mathf.FloorToInt(damage * 1.5f); // 받는 피해 증가
    }
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

public class VirtusoeStance : MaelStanceState
{
    public override void OnGiveDamage(ref int damage)
    {
        damage = Mathf.FloorToInt(damage * 1.5f); // 주는 피해 대폭 증가
    }
}

public class MaelPlayer : BattlePlayer
{
    private MaelStanceState currentStance;
    private Dictionary<MaelStanceType, StatusEffector> stanceEffector = new();
    private Dictionary<MaelStanceType, MaelStanceState> stanceMap = new();
    public MaelStanceType CurrentStanceType { get; private set; } = MaelStanceType.None;

    public override void Initialize()
    {
        base.Initialize();
        stanceMap[MaelStanceType.None] = new StancelessStance();
        stanceMap[MaelStanceType.Defense] = new DefenseStance();
        stanceMap[MaelStanceType.Offense] = new OffenseStance();
        stanceMap[MaelStanceType.Virtusoe] = new VirtusoeStance();
        
        var effectList = AssetManager.Instance.GetCharacterAssetTable(CharacterName).characterEffectorTable.AssetList;
        foreach (var effectorAsset in effectList)
        {
            var key = effectorAsset.EffectorName.ToLower();
            statusEffects[key] = effectorAsset.CreateEffector(this);
        }

        foreach (var stance in stanceMap.Values)
            stance.Initialize(this);

        ChangeStance(MaelStanceType.None); // 초기 태세
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

}
