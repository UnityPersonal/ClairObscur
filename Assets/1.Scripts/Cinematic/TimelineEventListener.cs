using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Timeline;

public class TimelineEventListener : MonoBehaviour
{
    static private TimelineEventListener _instance;

    static public TimelineEventListener Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TimelineEventListener>();
            }
            return _instance;
        }
        
    }
    
    public void OnDefendEndReceived()
    {
        // Defend End signal 수신 시 처리
        Debug.Log("OnDefendEndReceived");
        
        BattleManager.Instance.CurrentTurnCharacter.TargetCharacter.OnEmittedEndDefendSignal();
    }
    
    public void OnDefendReceived()
    {
        
        // Defend signal 수신 시 처리
        Debug.Log("OnDefendReceived");
        BattleManager.Instance.CurrentTurnCharacter.TargetCharacter.OnEmittedBeginDefendSignal();

    }
    
    public void OnAttackReceived()
    {
        BattleManager.Instance.CurrentTurnCharacter.OnEmittedBeginAttackSignal();
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
