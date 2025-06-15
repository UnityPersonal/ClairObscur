using System;
using Timeline.Samples;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
[TrackBindingType(typeof(Transform))]
[TrackClipType(typeof(MoveToTargetClip))]
[TrackColor(0.0f, 1.0f, 0.0f)]
public class MoveToTargetTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        // Create a ScriptPlayable for the MoveToTargetBehaviour mixer.
        return ScriptPlayable<MoveToTargetBehaviour>.Create(graph, inputCount);
    }
}
