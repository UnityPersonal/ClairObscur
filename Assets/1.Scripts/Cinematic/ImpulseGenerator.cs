using System;
using Unity.Cinemachine;
using UnityEngine;

public class ImpulseGenerator : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource[] impulseSources;

    private void OnEnable()
    {
        foreach (CinemachineImpulseSource impulseSource in impulseSources)
        {
            impulseSource.GenerateImpulse();
        }
    }
}
