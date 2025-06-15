using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class MoveToTargetClip : PlayableAsset , IPropertyPreview
{
    public ExposedReference<Transform> actor;
    public ExposedReference<Transform> target;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MoveToTargetBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();

        behaviour.target = target.Resolve(graph.GetResolver());
        behaviour.actor = actor.Resolve(graph.GetResolver());

        return playable;
    }

    public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
        const string kLocalPosition = "m_LocalPosition";
        const string kLocalRotation = "m_LocalRotation";

        driver.AddFromName<Transform>(kLocalPosition + ".x");
        driver.AddFromName<Transform>(kLocalPosition + ".y");
        driver.AddFromName<Transform>(kLocalPosition + ".z");
        driver.AddFromName<Transform>(kLocalRotation + ".x");
        driver.AddFromName<Transform>(kLocalRotation + ".y");
        driver.AddFromName<Transform>(kLocalRotation + ".z");
        driver.AddFromName<Transform>(kLocalRotation + ".w");
    }
}
