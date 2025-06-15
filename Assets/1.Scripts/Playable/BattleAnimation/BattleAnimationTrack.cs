using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;

[TrackColor(1f, 0.0990566f, 0.9230301f)]
[TrackClipType(typeof(BattleAnimationClip))]
[TrackBindingType(typeof(Animator))]
public class BattleAnimationTrack : AnimationTrack
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var mixer = base.CreateTrackMixer(graph,go, inputCount);
        // 추가 로직 삽입 가능
        Debug.Log("CustomAnimationTrack - Mixer Created");
        
        return mixer;
    }

    // Please note this assumes only one component of type Animator on the same gameobject.
    public override void GatherProperties (PlayableDirector director, IPropertyCollector driver)
    {
        base.GatherProperties (director, driver);
    }
}
