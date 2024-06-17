using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatIncrease {
    public int maxHealth;
    public int damage;
    public int attSpeed;
    public int MovSpeed;
}

[CreateAssetMenu()]
public class LevelUpInfoSO : ScriptableObject
{
    public int maxLevel;
    public int[] xpPerLevel;
    public StatIncrease[] statIncreasePerLevel;

    public StatIncrease GetStatIncreaseForLevel(int level) {
        // First level up is to lv2
        return statIncreasePerLevel[level - 2];
    }

    public int GetXPNeededForLevel(int level) {
        return level > maxLevel ? 0 : xpPerLevel[level - 2];
    }
}
