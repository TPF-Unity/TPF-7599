using System.Collections.Generic;
using System.Linq;
using AI.Sensors;
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
    [SerializeField] PlayerSensor playerSensor;
    [SerializeField] private ItemInfo[] itemsInfo;
    [SerializeField] private AttackInfo attackInfo;


    protected override Node SetupTree() {
        PatrolWaypoint[] keyWaypoints = GameObject.FindGameObjectsWithTag("KeySpawn").Select(position => new PatrolWaypoint(position.transform)).ToArray();
        PatrolWaypoint[] doorWaypoints = GameObject.FindGameObjectsWithTag("DoorSpawn").Select(position => new PatrolWaypoint(position.transform)).ToArray();

        Node root = new Selector(new List<Node>{
            new Sequence(new List<Node>{
                new TaskHPLowerThan(50f),
                new TaskEnemiesInRange(GetItemInfoForType(ItemType.Enemy)),
                new TaskKeepDistance()
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
                new TaskPlayerInRange(GetItemInfoForType(ItemType.Player), playerSensor),
                new TaskAttackPlayer(attackInfo)
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