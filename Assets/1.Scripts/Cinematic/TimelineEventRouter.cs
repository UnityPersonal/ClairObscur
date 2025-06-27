using UnityEngine;
using UnityEngine.Timeline;

public class TimelineEventRouter : MonoSingleton<TimelineEventRouter>
{
    public void OnSignalReceived(SignalAsset signalAsset)
    {
        string signal = signalAsset.name.ToLower();
        switch (signal)
        {
            case "qtepause" : QTESystem.Instance.OnQTEPauseReceived(); break;
            case "defendbegin" : BattleManager.Instance.CurrentTurnCharacter.Target.OnBeginDefendSignal(); break;
            case "defendend" : BattleManager.Instance.CurrentTurnCharacter.Target.OnEndDefendSignal(); break;
            case "attackbegin" : BattleManager.Instance.CurrentTurnCharacter.OnBeginAttackSignal(); break;
            case "counterbegin":
            {
                if (BattleManager.Instance.CurrentTurnCharacter is BattleMonster monster)
                    monster.OnCounterBeginSignal();
                break;
            }
            case "counterend":
            {
                if (BattleManager.Instance.CurrentTurnCharacter is BattleMonster monster)
                    monster.OnCounterEndSignal();
                break;
            }
            case "parryingattack": BattleManager.Instance.CurrentTurnCharacter.Target.OnCounterAttackSignal(); break;
            
            default: 
                throw new System.Exception("Unknown signal: " + signal);
        }
        // Signal 수신 시 처리
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
