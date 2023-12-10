using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArcherSkeletonAnimationController : NPCAnimationController
{

    public override void Initialize(Animator animator)
    {
        Action playWalkAnimation = () =>
        {
            animator.SetBool("isMoving", true);
        };

        Action playAttackAnimation = () =>
        {
            animator.SetBool("isMoving", false);
            animator.SetTrigger("Attack");
        };

        Action playIdleAnimation = () =>
        {
            animator.SetBool("isMoving", false);
        };

        animationClips = new Dictionary<AnimationType, Action>
        {
            { AnimationType.Walk, playWalkAnimation },
            { AnimationType.Attack, playAttackAnimation },
            { AnimationType.Idle, playIdleAnimation }
        };

    }

}