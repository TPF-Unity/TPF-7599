using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class EnemyNPCController : MonoBehaviour
{
    private Transform player;

    // Attacking
    public GameObject projectile;
    public LayerMask whatIsGround, whatIsPlayer;

    // Animation
    public Animator animator;
    protected NPCAnimationController animationController;

    // AI
    public UnityEngine.AI.NavMeshAgent agent;

    public Transform attackSpawnPoint;
    [SerializeField] protected NPCStats stats;

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
}
