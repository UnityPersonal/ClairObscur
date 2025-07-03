using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayActionArgs
{
    
    public BattleCharacter Actor { get; set; }
    public BattleCharacter Target { get; set; }
    
    public QTESystem QteSystem { get; set; }
    
    public Action<PlayableDirector> OnActionFinished { get; set; }
    
    public PlayActionArgs(
        BattleCharacter actor,
        BattleCharacter target,
        Action<PlayableDirector> callback = null
        )
    {
        Actor = actor;
        Target = target;
        OnActionFinished = callback;
    }
}

public class BattleActionController : MonoBehaviour
{
    [SerializeField] string actionName;
    public string ActionName { get => actionName.ToLower(); }
    
    public PlayableDirector director;
    
    // 액션 움직임에 참고하기 위한 위치 정보
    // 액션을 재생할때 target의 위치를 참고하여 옮겨준다. 
    [SerializeField] private Transform startLocation;
    [SerializeField] private Transform endLocation;
    
    [SerializeField] private CinematicActorReference startReference;
    [SerializeField] private CinematicActorReference endReference;
    
    [SerializeField] private TimelineActor actor;
    public TimelineActor Actor => actor;
    [SerializeField] private CinemachineTargetGroup targetGroup;
    
    private CinemachineCamera[] cameras;
    public QTEInteractUI[] qteInteractUIs;
    
    [SerializeField] private bool tweenBind = true;
    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        if (director == null)
        {
            Debug.LogError("PlayableDirector component is missing on BattleActionController.");
        }
        director.stopped += OnStopped;

        cameras = GetComponentsInChildren<CinemachineCamera>(true);
    }
    private void OnDestroy()
    {
        if (director != null)
        {
            director.stopped -= OnStopped;
        }
    }

    Action<PlayableDirector> onActionFinished = null;
    private void OnStopped(PlayableDirector obj)
    {
        onActionFinished?.Invoke(obj);
    }
    
    public void PauseAction()
    {
        director.Pause();
        foreach (var camera in cameras)
        {
            camera.enabled = false;
        }
    }
    
    public void ResumeAction()
    {
        director.Resume();
        foreach (var camera in cameras)
        {
            camera.enabled = true;
        }
    }

    public void StopAction()
    {
        director.Stop();
    }
    
    public void PlayAction(PlayActionArgs args)
    {
        if (tweenBind)
        {
            if (startLocation != null && args.Actor != null)
            {
                startLocation.position = args.Actor.transform.position;
            }

            if (endLocation != null && args.Target != null)
            {
                endLocation.position = args.Target.Actor.AttackTransform.position;
            }
                
        }

        if (targetGroup != null)
        {
            var actor = targetGroup.Targets[0];
            actor.Radius = args.Actor.FocusRadius;
            
            var target = targetGroup.Targets[1];
            target.Object.position = args.Target.Actor.TrackTransform.position;
            target.Radius = args.Target.FocusRadius;
        }

        var timeline = director.playableAsset as TimelineAsset;
        // 2. 모든 트랙 순회
        foreach (var track in timeline.GetOutputTracks())
        {
            if (track is CinemachineTrack)
            {
                var brain = Camera.main.GetComponent<CinemachineBrain>();
                director.SetGenericBinding(track,brain);
            }

            if (track is BattleSignalEmitTrack)
            {
                Debug.Log("BattleSignalEmitTrack found, binding to TimelineEventListener.");
                director.SetGenericBinding(track, TimelineEventRouter.Instance);
            }
        }

        onActionFinished = args.OnActionFinished;
        director.Play();
    }
    
}
