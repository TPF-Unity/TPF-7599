using UnityEngine;

public class Slash : MonoBehaviour
{
    public DamageLayerMapping damageLayerMapping;
    public float range = 4.0f;
    public float angle = 45.0f;

    public GameObject visualSpherePrefab;
    public float displayDuration = 0.5f;


    void OnDrawGizmos()
    {
        // Set the color of the Gizmo
        Gizmos.color = Color.red;

        // Draw a wireframe sphere representing the overlap sphere
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void PerformAttack()
    {
        ShowAttackRange();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hitCollider in hitColliders)
        {
            if (IsWithinAttackAngle(hitCollider.transform))
            {
                string attackerLayer = LayerMask.LayerToName(gameObject.layer);
                string targetLayer = LayerMask.LayerToName(hitCollider.gameObject.layer);
                Debug.Log("attackerLayer");
                Debug.Log(attackerLayer);
                Debug.Log("targetLayer");
                Debug.Log(targetLayer);
                if (damageLayerMapping.CanDamage(attackerLayer, targetLayer))
                {
                    Debug.Log("3");
                    if (hitCollider.gameObject.TryGetComponent(out VulnerableUnit target))
                    {
                        Debug.Log("it is damaging");
                        Debug.Log(target.ToString());
                        target.TakeDamage(10);
                    }
                }
            }
        }
    }

    private bool IsWithinAttackAngle(Transform target)
    {
        // Debug.Log("target");
        // Debug.Log(target.position);
        Vector3 directionToTarget = target.position - transform.position;
        // Debug.Log("directionToTarget");
        // Debug.Log(directionToTarget);
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        // Debug.Log("angleToTarget");
        // Debug.Log(angleToTarget);
        return angleToTarget < angle;
    }

    private void ShowAttackRange()
    {
        GameObject visualSphere = Instantiate(visualSpherePrefab, transform.position, Quaternion.identity);
        visualSphere.transform.localScale = new Vector3(range * 2, range * 2, range * 2);
        Destroy(visualSphere, displayDuration);
    }
}