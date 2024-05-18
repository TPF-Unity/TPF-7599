using System.Collections;
using System.Collections.Generic;
using Misc;
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
    [SerializeField] protected NPCStats stats;

    private void Awake()
    {
        // TODO: Find better way to get player
        var playerGameObject = GameObject.Find(GameObjects.Player.ToString());
        //var playerGameObject = transform.parent.parent.parent.Find("Player");
        if (playerGameObject != null)
        {
            player = playerGameObject.transform;
        }
        else
        {
            Debug.Log("Could not find Player reference");
        }

        animationController = GetComponent<NPCAnimationController>();
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
