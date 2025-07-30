using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;



[Serializable]
public class RndPlayableClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField] AnimationClip animationClip;
    public RndPlayableBehaviour template = new RndPlayableBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        //var playable = ScriptPlayable<RndPlayableBehaviour>.Create (graph, owner
        var playable = CreateRndPlayable(graph, animationClip);
        return playable;
    }
    
    Playable CreateRndPlayable (PlayableGraph graph, AnimationClip clip)
    {
        var clipPlayable = AnimationClipPlayable.Create(graph, clip);
        return ScriptPlayable<RndPlayableBehaviour>.Create (graph, template);
    }
}
