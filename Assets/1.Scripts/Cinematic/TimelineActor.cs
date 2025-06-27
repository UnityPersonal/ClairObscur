using UnityEngine;

public class TimelineActor : MonoBehaviour
{
    [SerializeField] private Transform trackTransform;
    [SerializeField] private Transform attackTransform;
    public Transform TrackTransform => trackTransform;
    public Transform AttackTransform => attackTransform;
    
    public float TrackRadius = 1f;
    
    public float TrackWeight = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
}
