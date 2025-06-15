using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// A behaviour that is attached to a playable
public class BatlleSignalEmitBehaviour : PlayableBehaviour
{
    public TimelineEventListener eventListener;
    //public SignalAsset signal;
    
    
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        /*if (signal != null && eventListener != null)
        {
            // Signal을 수동으로 발생
            eventListener.OnSignalReceived(signal);
        }*/
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        
    }
}
