using UnityEngine;
using UnityEngine.Events;

public class VulnerableUnit : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public UnityEvent<int> onHealthChanged;
    public UnityEvent onDied;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        onHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            onDied?.Invoke();
            Destroy(gameObject);
        }
    }
}
