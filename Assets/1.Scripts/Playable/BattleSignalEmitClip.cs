using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class BattleSignalEmitClip : PlayableAsset, ITimelineClipAsset
{
    /*public ExposedReference<TimelineEventListener> eventListener;
    public SignalAsset signal;*/
    
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<BatlleSignalEmitBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();    
        /*behaviour.eventListener = eventListener.Resolve(graph.GetResolver());
        behaviour.signal = signal;*/        

        return playable;
    }

    public ClipCaps clipCaps => ClipCaps.Extrapolation;
}
