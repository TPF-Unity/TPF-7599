using System;
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
    public UnitDifficultyManager unitDifficultyManager;

    public void Awake()
    {
        stats = Instantiate(baseStats);
    }

    public void Start()
    {
        stats.MaxHealth *= unitDifficultyManager.GetHealthMultiplier();
        stats.Health = stats.MaxHealth;
        stats.MovementSpeed *= unitDifficultyManager.GetSpeedMultiplier();
        stats.Damage *= unitDifficultyManager.GetDamageMultiplier();
        stats.AttackSpeed *= unitDifficultyManager.GetAttackSpeedMultiplier();
        stats.SightRange *= unitDifficultyManager.GetSightRangeMultiplier();
        stats.XPDropped = (int)(stats.XPDropped * unitDifficultyManager.GetXpDropMultiplier());
    }

    public void TakeDamage(float damage)
    {
        TakeDamageFrom(damage, null);
    }

    public void TakeDamageFrom(float damage, PlayerController player)
    {
        // Debug.Log("maxHealth");
        // Debug.Log(stats.MaxHealth);
        stats.TakeDamage(damage);
        onHealthChanged?.Invoke(stats.Health / stats.MaxHealth * 100);
        if (stats.Health <= 0)
        {
            onDied?.Invoke();
            OnDestroyed?.Invoke(this, EventArgs.Empty);
            if (player)
            {
                player.GainXP(stats.XPDropped);
            }
            Destroy(gameObject);
        }
    }
}
