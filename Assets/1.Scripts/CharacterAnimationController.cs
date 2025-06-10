using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] AnimationData animationData;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        var overrideController = new AnimatorOverrideController(animationData.overrideController);
        animator.runtimeAnimatorController = overrideController; 

        foreach (var clipData in animationData.animationClips)
        {
            overrideController[clipData.clipName] = clipData.clip;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
