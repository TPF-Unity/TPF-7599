using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WizardSkeletonAnimationController : NPCAnimationController
{

    public override void Initialize(Animator animator)
    {
        Action playWalkAnimation = () =>
        {
            animator.SetBool("move_forward", true);
            animator.SetBool("move_forward_fast", false);
        };

        Action playAttackAnimation = () =>
        {
            animator.SetBool("move_forward", false);
            animator.SetBool("move_forward_fast", false);
            animator.SetBool("idle_combat", true);
            animator.SetTrigger("attack_short_001");
        };

        Action playIdleAnimation = () =>
        {
            animator.SetBool("idle_combat", false);
        };

        animationClips = new Dictionary<AnimationType, Action>
        {
            { AnimationType.Walk, playWalkAnimation },
            { AnimationType.Attack, playAttackAnimation },
            { AnimationType.Idle, playIdleAnimation }
        };

    }

}