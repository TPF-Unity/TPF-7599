using BehaviourTree;
using Misc;
using UnityEngine;
using UnityEngine.AI;

public class TaskAttackPlayer : Node {
    private readonly AttackInfo attackInfo;
    private float cooldown = 0f;

    public TaskAttackPlayer(AttackInfo attackInfo) {
        this.attackInfo = attackInfo;
    }

    public override NodeState Evaluate() {
        Transform target = (Transform) GetData(BTContextKey.Player);
        cooldown -= Time.deltaTime;

        if (target != null) {
            Transform transform = (Transform) GetData(BTContextKey.Transform);
            NavMeshAgent navMeshAgent = (NavMeshAgent) GetData(BTContextKey.NavMeshAgent);

            transform.LookAt(target.position);

            if (Vector3.Distance(transform.position, target.position) >= attackInfo.attackRange) {
                navMeshAgent.SetDestination(target.position);
            }

            if (Vector3.Distance(transform.position, target.position) <= 0.9f * attackInfo.attackRange) {
                navMeshAgent.ResetPath();
            }

            if (cooldown <= 0f && Vector3.Distance(transform.position, target.position) <= attackInfo.attackRange) {
                NPCStats stats = ((Unit) GetData(BTContextKey.Unit)).stats;

                GameObject bullet = Object.Instantiate(attackInfo.bullet);
                Bullet instance = bullet.GetComponent<Bullet>();
                instance.Damage = stats.Damage;
                instance.gameObject.layer = LayerMask.NameToLayer(Layer.EnemyProjectiles.ToString());
                var bulletTransform = instance.transform;
                bulletTransform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * 1.0f;

                instance.Shoot(target.position);

                cooldown = 1 / stats.AttackSpeed;
            }

            state = NodeState.RUNNING;
            return state;
        } else {
            state = NodeState.FAILURE;
            return state;
        }
    }
}