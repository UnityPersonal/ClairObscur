using UnityEngine;

public class BattleWarrior : BattlePlayer
{
    /// <summary>
    /// 베르소의 시스템으로 확장한 전투용 플레이어 클래스입니다.
    /// 공격 또는 방어에 성공하면 카운트를 획득한다.
    /// 피해을 받으면 카운트를 잃는다.
    /// </summary>
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int GuardCount { get; private set; }= 0;// D C B A S
    
    void Start()
    {
        
    }

    protected override void OnTakedDamage()
    {
        base.OnTakedDamage();
        GuardCount -= 1;
    }

    protected override void OnDodged()
    {
        base.OnDodged();
        GuardCount += 1;
    }
    
    protected override void OnParried()
    {
        base.OnParried();
        GuardCount += 1;
    }
    
    protected override void OnJumped()
    {
        base.OnJumped();
        GuardCount += 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
