using UnityEngine;

[CreateAssetMenu(fileName = "AttackPatternData", menuName = "Scriptable Objects/AttackPatternData")]
public class AttackPatternData : ScriptableObject
{
    public string AttackPatternName;

    public AnimationClip AttackClip;
    
    [Header("Dodge Timing Range")]
    [Range(0,1)]
    public float DodgeStartTime = 0f;
    
    public bool IsJump = false;
}
