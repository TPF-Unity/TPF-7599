using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Classes.References;
using UnityEngine;

public enum AnimationType
{
    Walk,
    Attack,
    Idle
}

public abstract class NPCAnimationController : MonoBehaviour
{
    public Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
        this.Initialize(animator);
    }

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