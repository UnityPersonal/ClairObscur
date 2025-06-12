using UnityEngine;
using UnityEngine.Playables;

public class MoveToTargetBehaviour : PlayableBehaviour
{
    public Transform actor;
    public Transform target;
    public double duration;

    private Vector3 startPos;
    private Vector3 endPos;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (actor != null && target != null)
        {
            startPos = actor.position;
            endPos = target.position;
        }
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (actor == null || target == null) return;
        if(playerData == null) return;

        Transform trackBinding = playerData as Transform;
        if (trackBinding == null) return;
        
        double time = playable.GetTime();
        double t = time / playable.GetDuration();
        trackBinding.position = Vector3.Lerp(startPos, endPos, (float)t);
        //Debug.Log(trackBinding.position);
    }
}