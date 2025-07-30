using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class RndPlayableBehaviour : PlayableBehaviour
{
    public bool runInEditMode;
    public bool enabled;
    public bool useGUILayout;
}
