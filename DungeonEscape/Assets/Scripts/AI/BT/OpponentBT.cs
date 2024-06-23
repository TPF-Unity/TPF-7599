using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;


public enum ItemType {
    Key,
    Door,
    Player,
    PowerUp,
    Enemy,
}

[System.Serializable]
public class ItemInfo
{
    public ItemType itemType;
    public float detectionRange;
    public LayerMask layer;
}

[System.Serializable]
public class AttackInfo
{
    public float attackRange;
    public float bulletSpeed;
    public GameObject bullet;
    public LayerMask attackableLayerMask;
    public DamageLayerMapping damageLayerMapping;
}

public class PatrolWaypoint
{
    public Transform transform;
    public bool visited;

    public PatrolWaypoint(Transform transform) {
        this.transform = transform;
        visited = false;
    }
}


[RequireComponent(typeof(NavMeshAgent))]
public class OpponentBT : BehaviourTree.Tree {
    [SerializeField] private ItemInfo[] itemsInfo;
    [SerializeField] private AttackInfo attackInfo;


    protected override Node SetupTree() {
        PatrolWaypoint[] patrolWaypoints = GameObject.FindGameObjectsWithTag("PatrolPoint").Select(position => new PatrolWaypoint(position.transform)).ToArray();
        PatrolWaypoint[] keyWaypoints = GameObject.FindGameObjectsWithTag("KeySpawn").Select(position => new PatrolWaypoint(position.transform)).ToArray().Concat(patrolWaypoints).ToArray();
        PatrolWaypoint[] doorWaypoints = GameObject.FindGameObjectsWithTag("DoorSpawn").Select(position => new PatrolWaypoint(position.transform)).ToArray().Concat(patrolWaypoints).ToArray();

        Node root = new Selector(new List<Node>{
            new Selector(new List<Node>{
                new TaskIsEscaping(),
                new Sequence(new List<Node>{
                    new TaskEnemiesInRange(GetItemInfoForType(ItemType.Enemy)),
                    new TaskHPLowerThan(0.30f),
                    new TaskEscape(patrolWaypoints, GetItemInfoForType(ItemType.Enemy))
                })
            }),
            new Sequence(new List<Node>{
                new TaskDoorInRange(GetItemInfoForType(ItemType.Door)),
                new TaskKeysCollected(),
                new TaskGoToDoor()
            }),
            new Sequence(new List<Node>{
                new TaskKeyInRange(GetItemInfoForType(ItemType.Key)),
                new TaskGoToKey()
            }),
            new Sequence(new List<Node>{
                new TaskHPLowerThan(0.80f),
                new TaskCheckPowerUpInRange(GetItemInfoForType(ItemType.PowerUp)),
                new TaskGoToPowerUp()
            }),
            new Sequence(new List<Node>{
                new TaskEnemiesInRange(GetItemInfoForType(ItemType.Enemy)),
                new TaskAttackEnemies(attackInfo)
            }),
            new Selector(new List<Node>{
                new Sequence(new List<Node>{
                    new TaskKeysCollected(),
                    new TaskPatrol(doorWaypoints),
                }),
                new Sequence(new List<Node>{
                    new Inverter(new TaskKeysCollected()),
                    new TaskPatrol(keyWaypoints),
                })
            })
        });

        root.SetData(BTContextKey.Root, root);
        root.SetData(BTContextKey.Transform, transform);
        root.SetData(BTContextKey.NavMeshAgent, GetComponent<NavMeshAgent>());
        root.SetData(BTContextKey.Unit, GetComponent<Unit>());

        return root;
    }

    private ItemInfo GetItemInfoForType(ItemType type) {
        return itemsInfo.Where(item => item.itemType == type).FirstOrDefault();
    }
}