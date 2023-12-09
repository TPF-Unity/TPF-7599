using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    public UnitStats stats;
    public UnityEvent<int> onHealthChanged;
    public UnityEvent onDied;

    public void Start()
    {
        stats = GetComponent<UnitStats>();
    }

    public void TakeDamage(int damage)
    {
        stats.CurrentHealth -= damage;
        onHealthChanged?.Invoke(stats.CurrentHealth);

        if (stats.CurrentHealth <= 0)
        {
            Debug.Log("destroy");
            onDied?.Invoke();
            Destroy(gameObject);
        }
    }
}