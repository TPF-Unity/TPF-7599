using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Classes.References;
using UnityEngine;

public enum AnimationType
{
    Walk,
    Attack,
    Idle,
    RangedAttack
}

public abstract class NPCAnimationController : MonoBehaviour
{
    public Animator animator;
    protected Unit unit;

    public void Start()
    {
        animator = GetComponent<Animator>();
        unit = GetComponent<Unit>();
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

    public bool CanAttack()
    {
        return animator.GetBool("Shoot");
    }

    public void SetAttack(bool value)
    {
        animator.SetBool("Shoot", value);
    }
}