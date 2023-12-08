using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Kryz.CharacterStats;

[CreateAssetMenu(fileName = "BaseUnitStats", menuName = "Character/Base Unit Stats", order = 1)]
public class BaseUnitStats : ScriptableObject
{
    public CharacterStat health;
    public Dictionary<Stat, CharacterStat> InitializeStats()
    {
        Dictionary<Stat, CharacterStat> stats = new Dictionary<Stat, CharacterStat>
        {
            { Stat.Health, health },
        };
        return stats;
    }
}
