using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class EnemyNPCController : MonoBehaviour
{
    public Transform player;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public GameObject projectile;
    public LayerMask whatIsGround, whatIsPlayer;
    
    protected bool alreadyAttacked;

    // States
    public bool playerInSightRange, playerInAttackRange;

    // Animation
    public Animator animator;
    protected NPCAnimationController animationController;

    // AI
    public UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] protected NPCStats stats;

    protected abstract void AttackPlayer();

    private void Awake()
    {
        animationController = GetComponent<NPCAnimationController>();
        player = GameObject.Find("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, stats.AttackRange);
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, stats.SightRange);
    // }

    protected void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
