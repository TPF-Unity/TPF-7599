using UnityEngine;

namespace AI.Sensors
{
    public class PlayerSensor : MonoBehaviour
    {
        public SphereCollider Collider;

        public delegate void PlayerEnterEvent(Transform player);

        public delegate void PlayerExitEvent(Vector3 lastKnownPosition);

        public event PlayerEnterEvent OnPlayerEnter;
        public event PlayerExitEvent OnPlayerExit;

        public DamageLayerMapping damageLayerMapping;

        private void Awake()
        {
            Collider = GetComponent<SphereCollider>();
        }

        private void EngageTarget(Collider other)
        {
            if (other.TryGetComponent(out Unit player) &&
                damageLayerMapping.CanDamage(LayerMask.LayerToName(gameObject.layer),
                    LayerMask.LayerToName(other.gameObject.layer)))
            {
                Vector3 directionToTarget =
                    (other.transform.position - transform.position).normalized;
                float distanceToTarget = Vector3.Distance(transform.position, other.transform.position);
                directionToTarget.y = 0f;
                Debug.DrawLine(transform.position + new Vector3(0, 1, 0), transform.position + directionToTarget * 10f,
                    Color.red,
                    duration: 1.0f);
                if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), directionToTarget,
                        out RaycastHit hitInfo,
                        distanceToTarget + 3.0f))
                {
                    if (hitInfo.collider.gameObject == other.gameObject)
                    {
                        OnPlayerEnter?.Invoke(other.gameObject.transform.root);
                    }
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            EngageTarget(other);
        }

        private void OnTriggerEnter(Collider other)
        {
            EngageTarget(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                OnPlayerExit?.Invoke(other.transform.position);
            }
        }

        public void SetRadius(float radius) {
            Collider.radius = radius;
        }
    }
}