using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;

[TrackColor(0.8907518f, 0f, 1f)]
[TrackClipType(typeof(RndPlayableClip))]
[TrackBindingType(typeof(RndAnimator))]
public class RndPlayableTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<RndPlayableMixerBehaviour>.Create (graph, inputCount);
    }

    // Please note this assumes only one component of type RndAnimator on the same gameobject.
    public override void GatherProperties (PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        RndAnimator trackBinding = director.GetGenericBinding(this) as RndAnimator;
        if (trackBinding == null)
            return;

        // These field names are procedurally generated estimations based on the associated property names.
        // If any of the names are incorrect you will get a DrivenPropertyManager error saying it has failed to register the name.
        // In this case you will need to find the correct backing field name.
        // The suggested way of finding the field name is to:
        // 1. Make sure your scene is serialized to text.
        // 2. Search the text for the track binding component type.
        // 3. Look through the field names until you see one that looks correct.
        driver.AddFromName<RndAnimator>(trackBinding.gameObject, "m_RunInEditMode");
        driver.AddFromName<RndAnimator>(trackBinding.gameObject, "m_Enabled");
        driver.AddFromName<RndAnimator>(trackBinding.gameObject, "m_UseGUILayout");
#endif
        base.GatherProperties (director, driver);
    }
}
