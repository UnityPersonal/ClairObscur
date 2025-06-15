using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class BattleAnimationClip : AnimationPlayableAsset
{
    public float customSpeed = 1.0f;
    public ExposedReference<AnimationClip> exposedClip;
    public float startTime;
    
    public BattleAnimationBehaviour template = new BattleAnimationBehaviour ();

    public ClipCaps clipCaps => ClipCaps.Blending | ClipCaps.SpeedMultiplier;

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var basePlayable = base.CreatePlayable(graph, owner);
        return basePlayable;
    }
}
