using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class EnemyNPCController : MonoBehaviour
{
    // Attacking
    public GameObject projectile;
    public LayerMask whatIsGround, whatIsPlayer;

    // Animation
    public Animator animator;
    protected NPCAnimationController animationController;

    // AI
    public UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] protected NPCStats stats;

    private void Awake()
    {
        animationController = GetComponent<NPCAnimationController>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
}
