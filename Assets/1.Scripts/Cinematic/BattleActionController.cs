using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BattleActionController : MonoBehaviour
{
    public PlayableDirector director;
    public TimelineAsset timelineAsset => director.playableAsset as TimelineAsset;
    
    public void Initialize()
    {
        director = GetComponent<PlayableDirector>();
        if (director == null)
        {
            Debug.LogError("PlayableDirector component is missing on BattleActionController.");
        }
        
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
