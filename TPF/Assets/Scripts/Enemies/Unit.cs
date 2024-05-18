using System;
using Misc;
using StarterAssets;
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

    public void TakeDamage(float damage) {
        TakeDamageFrom(damage, null);
    }

    public void TakeDamageFrom(float damage, PlayerController player)
    {
        stats.TakeDamage(damage);
        onHealthChanged?.Invoke(stats.Health / stats.MaxHealth * 100);
        if (stats.Health <= 0)
        {
            onDied?.Invoke();
            OnDestroyed?.Invoke(this, EventArgs.Empty);
            if (player) {
                player.GainXP(stats.XPDropped);
            }

            if (!gameObject.CompareTag(Tags.Player.ToString()))
            {
                Destroy(gameObject);
            }

        }
    }
}
