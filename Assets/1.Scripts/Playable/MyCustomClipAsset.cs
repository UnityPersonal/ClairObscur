using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MyCustomClipAsset : PlayableAsset , ITimelineClipAsset
{
    public AnimationClip myClip;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return AnimationClipPlayable.Create(graph, myClip);
    }

    public ClipCaps clipCaps => ClipCaps.All;
}
