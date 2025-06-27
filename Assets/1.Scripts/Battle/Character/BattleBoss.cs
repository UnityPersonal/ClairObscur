public class BattleBoss : BattleMonster
{
    public override void Initialize()
    {
        base.Initialize();
        BattleScreenUI.Instance.SetBossHpBar(this);
    }
        
}