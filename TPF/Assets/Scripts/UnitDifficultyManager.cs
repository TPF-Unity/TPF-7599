using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDifficultyManager : MonoBehaviour
{

    public AnimationCurve healthMultiplier;
    public AnimationCurve speedMultiplier;
    public AnimationCurve damageMultiplier;
    public AnimationCurve attackSpeedMultiplier;
    public AnimationCurve sightRangeMultiplier;
    public AnimationCurve xpDropMultiplier;
    public GameManager gameManager;
    public float GetHealthMultiplier()
    {
        return (float)healthMultiplier.Evaluate(GameManager.instance.GetDifficulty());
    }

    public float GetSpeedMultiplier()
    {
        return (float)speedMultiplier.Evaluate(GameManager.instance.GetDifficulty());
    }

    public float GetDamageMultiplier()
    {
        return (float)damageMultiplier.Evaluate(GameManager.instance.GetDifficulty());
    }

    public float GetAttackSpeedMultiplier()
    {
        return (float)attackSpeedMultiplier.Evaluate(GameManager.instance.GetDifficulty());
    }

    public float GetSightRangeMultiplier()
    {
        return (float)sightRangeMultiplier.Evaluate(GameManager.instance.GetDifficulty());
    }

    public float GetXpDropMultiplier()
    {
        return (float)xpDropMultiplier.Evaluate(GameManager.instance.GetDifficulty());
    }
}
