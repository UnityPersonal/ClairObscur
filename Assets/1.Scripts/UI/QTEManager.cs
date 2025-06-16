using System;
using UnityEngine;

public class QTEManager : MonoBehaviour
{
    [SerializeField] private QTEInteractUI[] qteUIList; // List of QTE interact UIs
    
    private int currentQteIndex = 0; // Index of the current QTE UI being displayed

    private void OnDisable()
    {
        foreach (var qteUI in qteUIList)
        {
            qteUI.gameObject.SetActive(false);
        }
    }

    public void ActivateQTE() // 순서대로 등록되어 있는 QTE UI를 활성화 시킨다.
    {
        if (currentQteIndex < 0 || currentQteIndex >= qteUIList.Length)
        {
            Debug.LogError("QTE index out of range: " + currentQteIndex);
            return;
        }
        
        // Activate the specified QTE UI
        qteUIList[currentQteIndex].gameObject.SetActive(true);
    }
    
    public bool IsQTESuccess(int qteIndex)
    {
        return qteUIList[qteIndex].IsSuccess;
    }

    private void Update()
    {
        // 활성화 되어 있는 QTE UI를 시간에 따라 업데이트 하고,
        // 만약
    }
}
