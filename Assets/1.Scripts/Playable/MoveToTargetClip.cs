using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class MoveToTargetClip : PlayableAsset
{
    public ExposedReference<Transform> actor;
    public ExposedReference<Transform> target;
    
    public double duration = 1.0;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MoveToTargetBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();

        behaviour.target = target.Resolve(graph.GetResolver());
        behaviour.actor = actor.Resolve(graph.GetResolver());
        behaviour.duration = duration;

        return playable;
    }
}
