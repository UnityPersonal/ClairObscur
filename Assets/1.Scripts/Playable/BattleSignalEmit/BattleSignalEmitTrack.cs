using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
[TrackBindingType(typeof(TimelineEventRouter))]
[TrackClipType(typeof(BattleSignalEmitClip))]
[TrackColor(0.2f, 0.6f,0.15f)]
public class BattleSignalEmitTrack : TrackAsset
{
    /*public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        // Create a ScriptPlayable for the BatlleSignalEmitBehaviour mixer.
        return ScriptPlayable<BatlleSignalEmitBehaviour>.Create(graph, inputCount);
    }*/
}
