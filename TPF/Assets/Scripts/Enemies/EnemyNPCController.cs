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
    public LayerMask whatIsGround, whatIsPlayer, whatIsWall;

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

    protected virtual void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, stats.SightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, stats.AttackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            animationController.PlayAnimation(AnimationType.Walk);
            Patroling();
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            animationController.PlayAnimation(AnimationType.Walk);
            ChasePlayer();
        }
        if (playerInAttackRange && playerInSightRange)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            Debug.DrawRay(transform.position, directionToPlayer, Color.blue);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, stats.AttackRange, whatIsWall))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
                animationController.PlayAnimation(AnimationType.Walk);
                ChasePlayer();
            }
            else
            {
                Debug.DrawLine(transform.position, hit.point, Color.green);
                AttackPlayer();
            }
        }
    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
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
