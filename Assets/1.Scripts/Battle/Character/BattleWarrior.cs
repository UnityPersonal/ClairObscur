using Unity.VisualScripting;
using UnityEngine;

public class BattleWarrior : BattlePlayer
{
    /// <summary>
    /// 베르소의 시스템으로 확장한 전투용 플레이어 클래스입니다.
    /// 공격 또는 방어에 성공하면 카운트를 획득한다.
    /// 피해을 받으면 카운트를 잃는다.
    /// </summary>
    public const int MAX_PERFECTION_COUNT = 1; // D C B A S
    public const int MAX_RANK = 4; // D C B A S
    public float CurrentGauge => PerfectionCount / (float)MAX_PERFECTION_COUNT; // 0 ~ 100
    
    const string RANK = "Rank";
    private const string PERFECTION = "Perfection";
    public int PerfectionCount
    {
        get => Stat(PERFECTION).StatValue;
        private set => Stat(PERFECTION).StatValue = value;
    }

    public int CurrentRankIndex
    {
        get => Stat(RANK).StatValue;
        set => Stat(RANK).StatValue = value;
    }        
    
    public override void ReadyBattle()
    {
        base.ReadyBattle();
        var effectList = AssetManager.Instance.GetCharacterAssetTable(CharacterName).characterEffectorTable.AssetList;
        foreach (var effectorAsset in effectList)
        {
            var key = effectorAsset.EffectorName.ToLower();
            statusEffects[key] = effectorAsset.CreateEffector(this);
        }
        
        var callbacks = BattleEventManager.Callbacks;
        callbacks.OnTakeDamage += OnTakeDamage;
    }
    
    public override void Activate()
    {
        base.Activate();
        WarriorUI.Instance.gameObject.SetActive(true);
    }
    
    public override void Deactivate()
    {
        base.Deactivate();
        WarriorUI.Instance.gameObject.SetActive(false);
    }

    protected override int GetCurrentDamage()
    {
        int damage = base.GetCurrentDamage();

        damage += CurrentRankIndex * 5;
        return damage;
    }

    public void ChangePerfectionCount(int value)
    {
        PerfectionCount = (PerfectionCount + value);
        if(PerfectionCount < 0)
        {
            if (CurrentRankIndex != 0)
            {
                CurrentRankIndex = (CurrentRankIndex - 1);
                PerfectionCount = MAX_PERFECTION_COUNT;
            }
            else
            {
                PerfectionCount = 0; // 최소값은 0
            }
        }
        else if(PerfectionCount > MAX_PERFECTION_COUNT)
        {
            if (CurrentRankIndex < (MAX_RANK) )
            {
                CurrentRankIndex = (CurrentRankIndex + 1);
                PerfectionCount = 0;
            }
            else
            {
                PerfectionCount = MAX_PERFECTION_COUNT; // 최대값은 MAX_GUARD_COUNT
            }
        }
        WarriorUI.Instance.SetGauge(CurrentGauge);
        WarriorUI.Instance.SetRank(CurrentRankIndex);
    }
    
    private void OnTakeDamage(TakeDamageEventArgs args)
    {
        if ((Activated == true) &&  (args.Target == PlayerTargetCharacter))
        {
            ChangePerfectionCount(1);
        }
    }
    
    protected override void OnTakedDamage(int damage)
    {
        base.OnTakedDamage(damage);
        ChangePerfectionCount(-1);
    }

    protected override void OnDodged()
    {
        base.OnDodged();
        ChangePerfectionCount(1);
    }
    
    protected override void OnParried()
    {
        base.OnParried();
        ChangePerfectionCount(1);
    }
    
    protected override void OnJumped()
    {
        base.OnJumped();
        ChangePerfectionCount(1);
    }
    
}
