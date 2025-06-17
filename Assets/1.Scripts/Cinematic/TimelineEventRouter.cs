using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Timeline;

public class TimelineEventRouter : MonoSingleton<TimelineEventRouter>
{
    public void OnDefendEndReceived()
    {
        BattleManager.Instance.CurrentTurnCharacter.TargetCharacter.OnEndDefendSignal();
    }
    
    public void OnDefendReceived()
    {
        BattleManager.Instance.CurrentTurnCharacter.TargetCharacter.OnBeginDefendSignal();
    }

    public void OnAttackReceived()
    {
        BattleManager.Instance.CurrentTurnCharacter.OnBeginAttackSignal();
    }
    
    public void OnParrayingAttackReceived()
    {
        // Parraying Attack signal 수신 시 처리
        BattleManager.Instance.CurrentTurnCharacter.TargetCharacter.OnCounterAttackSignal();
    }
    
    public void OnCoutnerBeginReceived()
    {
        // Counter Begin signal 수신 시 처리
        Debug.Log("OnCoutnerBeginReceived");
        if (BattleManager.Instance.CurrentTurnCharacter is BattleMonster monster)
        {
            monster.OnCounterBeginSignal();
        }
    }
    
    public void OnCoutnerEndReceived()
    {
        // Counter End signal 수신 시 처리
        Debug.Log("OnCoutnerEndReceived");
        if (BattleManager.Instance.CurrentTurnCharacter is BattleMonster monster)
        {
            monster.OnCounterEndSignal();
        }
    }
    
    
    public void OnSignalReceived(SignalAsset signal)
    {
        Debug.Log($"OnSignalReceived:  {signal.name}  ");
        // Signal 수신 시 처리
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
