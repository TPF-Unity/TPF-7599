using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OpponentAnimationController : NPCAnimationController
{

    private int animationIsMoving;
    private int animationMeleeAttack;
    private int animationRangedAttack;

    private void AssignAnimationIDs()
    {
        animationIsMoving = Animator.StringToHash("isMoving");
        animationMeleeAttack = Animator.StringToHash("MeleeAttack");
        animationRangedAttack = Animator.StringToHash("RangedAttack");
    }

    private void Awake()
    {
        AssignAnimationIDs();
    }

    public override void Initialize(Animator animator)
    {
        Action playWalkAnimation = () =>
        {
            animator.SetBool(animationIsMoving, true);
        };

        Action playMeleeAttackAnimation = () =>
        {
            animator.SetBool(animationIsMoving, false);
            animator.SetTrigger(animationMeleeAttack);
        };

        Action playIdleAnimation = () =>
        {
            animator.SetBool(animationIsMoving, false);
        };

        Action playRangedAttackAnimation = () =>
        {
            animator.SetBool(animationIsMoving, false);
            animator.SetTrigger(animationRangedAttack);
        };

        animationClips = new Dictionary<AnimationType, Action>
        {
            { AnimationType.Walk, playWalkAnimation },
            { AnimationType.Attack, playMeleeAttackAnimation },
            { AnimationType.RangedAttack, playRangedAttackAnimation },
            { AnimationType.Idle, playIdleAnimation }
        };

    }

}