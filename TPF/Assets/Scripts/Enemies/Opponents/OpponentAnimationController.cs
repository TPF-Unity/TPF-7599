using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OpponentAnimationController : NPCAnimationController
{

    private int animationIsMoving;
    private int animationIsMovingFast;
    private int animationIdleCombat;
    private int animationAttack;

    private void AssignAnimationIDs()
    {
        animationIsMoving = Animator.StringToHash("op_move_forward");
        animationIsMovingFast = Animator.StringToHash("op_move_forward_fast");
        animationIdleCombat = Animator.StringToHash("op_idle_combat");
        animationAttack = Animator.StringToHash("op_attack_short_001");
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
            animator.SetBool(animationIsMovingFast, false);
        };

        Action playAttackAnimation = () =>
        {
            animator.SetBool(animationIsMoving, false);
            animator.SetBool(animationIsMovingFast, false);
            animator.SetBool(animationIdleCombat, true);
            animator.SetBool(animationAttack, true);
        };

        Action playIdleAnimation = () =>
        {
            animator.SetBool(animationIdleCombat, false);
        };

        animationClips = new Dictionary<AnimationType, Action>
        {
            { AnimationType.Walk, playWalkAnimation },
            { AnimationType.Attack, playAttackAnimation },
            { AnimationType.Idle, playIdleAnimation }
        };

    }

}