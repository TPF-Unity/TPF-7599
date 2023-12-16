using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    public NPCStats baseStats;
    public NPCStats stats;
    public UnityEvent<float> onHealthChanged;
    public UnityEvent onDied;
    public static EventHandler OnDestroyed;
    
    public void Awake()
    {
        stats = Instantiate(baseStats);
    }

    public void TakeDamage(float damage)
    {
        stats.TakeDamage(damage);
        onHealthChanged?.Invoke(stats.Health);
        if (stats.Health <= 0)
        {
            onDied?.Invoke();
            OnDestroyed?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }
}
