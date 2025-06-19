using UnityEngine;

public class TimelineActor : MonoBehaviour
{
    [SerializeField] private Transform trackTransform;
    public Transform TrackTransform => trackTransform;
    
    public float TrackRadius = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
}
