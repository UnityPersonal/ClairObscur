using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BattleAnimationMixerBehaviour : PlayableBehaviour
{
    float m_DefaultSpeed;
    bool m_DefaultAnimatePhysics;

    float m_AssignedSpeed;
    bool m_AssignedAnimatePhysics;

    Animator m_TrackBinding;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        m_TrackBinding = playerData as Animator;

        if (m_TrackBinding == null)
            return;

        if (!Mathf.Approximately(m_TrackBinding.speed, m_AssignedSpeed))
            m_DefaultSpeed = m_TrackBinding.speed;
        if (m_TrackBinding.animatePhysics != m_AssignedAnimatePhysics)
            m_DefaultAnimatePhysics = m_TrackBinding.animatePhysics;

        int inputCount = playable.GetInputCount ();

        float blendedSpeed = 0f;
        float totalWeight = 0f;
        float greatestWeight = 0f;
        int currentInputs = 0;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<BattleAnimationBehaviour> inputPlayable = (ScriptPlayable<BattleAnimationBehaviour>)playable.GetInput(i);
            BattleAnimationBehaviour input = inputPlayable.GetBehaviour ();
            
            blendedSpeed += input.speed * inputWeight;
            totalWeight += inputWeight;

            if (inputWeight > greatestWeight)
            {
                m_AssignedAnimatePhysics = input.animatePhysics;
                m_TrackBinding.animatePhysics = m_AssignedAnimatePhysics;
                greatestWeight = inputWeight;
            }

            if (!Mathf.Approximately (inputWeight, 0f))
                currentInputs++;
        }

        m_AssignedSpeed = blendedSpeed + m_DefaultSpeed * (1f - totalWeight);
        m_TrackBinding.speed = m_AssignedSpeed;

        if (currentInputs != 1 && 1f - totalWeight > greatestWeight)
        {
            m_TrackBinding.animatePhysics = m_DefaultAnimatePhysics;
        }
    }
}
