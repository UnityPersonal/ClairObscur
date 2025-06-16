using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Timeline;

public class TimelineEventListener : MonoSingleton<TimelineEventListener>
{
    public void OnDefendEndReceived()
    {
        // Defend End signal 수신 시 처리
        Debug.Log("OnDefendEndReceived");
        
        BattleManager.Instance.CurrentTurnCharacter.TargetCharacter.OnEndDefendSignal();
    }
    
    public void OnDefendReceived()
    {
        
        // Defend signal 수신 시 처리
        Debug.Log("OnDefendReceived");
        BattleManager.Instance.CurrentTurnCharacter.TargetCharacter.OnBeginDefendSignal();
    }

    public void OnAttackReceived()
    {
        BattleManager.Instance.CurrentTurnCharacter.OnBeginAttackSignal();
    }
    
    public void OnCheckParriedReceived()
    {
        // Check Parried signal 수신 시 처리
        Debug.Log("OnCheckParriedReceived");
        // 패링 반격 여부를 체크하는 타이밍
        // 패링에 성공했다면 반격 액션으로 전환된다.
    }
    
    public void OnParrayingAttackReceived()
    {
        // Parraying Attack signal 수신 시 처리
        Debug.Log("OnParrayingAttackReceived");
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
