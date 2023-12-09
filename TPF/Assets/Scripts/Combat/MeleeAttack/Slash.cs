using UnityEngine;

public class Slash : MonoBehaviour
{
    public DamageLayerMapping damageLayerMapping;
    public float range = 4.0f;
    public float angle = 45.0f;

    public GameObject visualSpherePrefab;
    public float displayDuration = 0.5f;

    public void Execute()
    {
        ShowAttackRange();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hitCollider in hitColliders)
        {
            if (IsWithinAttackAngle(hitCollider.transform))
            {
                string attackerLayer = LayerMask.LayerToName(gameObject.layer);
                string targetLayer = LayerMask.LayerToName(hitCollider.gameObject.layer);
                if (damageLayerMapping.CanDamage(attackerLayer, targetLayer))
                {
                    if (hitCollider.gameObject.TryGetComponent(out Unit target))
                    {
                        target.TakeDamage(10);
                    }
                }
            }
        }
    }

    private bool IsWithinAttackAngle(Transform target)
    {
        Vector3 directionToTarget = target.position - transform.position;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        return angleToTarget < angle;
    }

    private void ShowAttackRange()
    {
        GameObject visualSphere = Instantiate(visualSpherePrefab, transform.position, Quaternion.identity);
        visualSphere.transform.localScale = new Vector3(range * 2, range * 2, range * 2);
        Destroy(visualSphere, displayDuration);
    }
}