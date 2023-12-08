
using System.Collections.Generic;
using Kryz.CharacterStats;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public BaseUnitStats baseStatsTemplate;
    private Dictionary<Stat, CharacterStat> stats;
    private int currentHealth;

    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            int maxHealth = (int)stats[Stat.Health].Value;
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }
    }

    void Start()
    {
        stats = baseStatsTemplate.InitializeStats();
        CurrentHealth = (int)stats[Stat.Health].Value;
    }

    public CharacterStat GetStat(Stat stat)
    {
        return stats[stat];
    }
}
