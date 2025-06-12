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
    
    public void OnDefendReceived()
    {
        // Defend signal 수신 시 처리
        Debug.Log("OnDefendReceived");
    }
    
    public void OnAttackReceived()
    {
        // Attack signal 수신 시 처리
        Debug.Log("OnAttackReceived");
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
