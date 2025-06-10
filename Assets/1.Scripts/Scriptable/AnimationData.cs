using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationData", menuName = "Scriptable Objects/AnimationData")]
public class AnimationData : ScriptableObject
{
    public string animationName;
    
    public AnimatorOverrideController overrideController;

    [Serializable]
    public class ClipData
    {
        public string clipName;
        public AnimationClip clip;
    }
    
    public ClipData[] animationClips;
}
