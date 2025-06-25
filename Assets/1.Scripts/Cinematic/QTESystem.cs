using System;
using System.Collections.Generic;
using UnityEngine;

public class QTESystem : MonoSingleton<QTESystem>
{
    public List<QTEInteractUI> qteQueue = new List<QTEInteractUI>();
    public void RegistQTE(QTEInteractUI qte)
    {
        qteQueue.Add(qte);
    }

    public void UnRegistQTE(QTEInteractUI qte)
    {
        qteQueue.Remove(qte);
        
        if ((qteQueue.Count == 0) && needResume)
        {
            Debug.Log("UnRegistQTE and needResume is true, resuming action");
            needResume = false;
            BattleManager.Instance.CurrentTurnCharacter.ResumeAction();
        }
    }

    private bool needResume = false;

    public void OnQTEPauseReceived()
    {
        Debug.Log("OnQTEPauseReceived");
        needResume = true;
        BattleManager.Instance.CurrentTurnCharacter.PauseAction();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (qteQueue.Count > 0)
            {
                var qte =  qteQueue[0];
                UnRegistQTE(qte);
                qte.EndQTE(Time.time);
            }
           
        }
    }
}