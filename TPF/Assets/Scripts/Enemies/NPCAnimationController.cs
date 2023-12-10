using System;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    Walk,
    Attack,
    Idle
}

public abstract class NPCAnimationController
{
    protected Dictionary<AnimationType, Action> animationClips = new Dictionary<AnimationType, Action>();

    public abstract void Initialize(Animator animator);
    public void PlayAnimation(AnimationType type)
    {
        if (animationClips.TryGetValue(type, out Action clip))
        {
            clip.Invoke();
        }
    }
}
