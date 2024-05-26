using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DifficultyManager : MonoBehaviour
{
    public float customDifficulty;
    public float difficultyMultiplier = 1.0f;
    public const int recentDifficultySamples = 5;
    public float[] recentDifficulty = new float[recentDifficultySamples];
    public int recentDifficultyIndex = 0;
    public float difficultyRangeThreshold = 0.3f;
    public int consecutiveWins = 0;
    public int consecutiveLosses = 0;

    public void SetDifficulty(string difficulty)
    {
        PlayerPrefs.SetString("Difficulty", difficulty);
    }

    public void SetCustomDifficulty(float customDifficulty)
    {
        this.customDifficulty = customDifficulty;
    }

    public float GetDifficulty()
    {
        string difficulty = PlayerPrefs.GetString("Difficulty", "NORMAL");
        switch (difficulty)
        {
            case "EASY":
                return -1;
            case "NORMAL":
                return 0;
            case "HARD":
                return 1;
            case "CUSTOM":
                return customDifficulty;
            default:
                return 0;
        }
    }

    public void MatchResult(bool win)
    {
        if (PlayerPrefs.GetString("Difficulty", "NORMAL") != "CUSTOM") return;
        if (win)
        {
            consecutiveWins++;
            consecutiveLosses = 0;
            AdjustScore(true);
        }
        else
        {
            consecutiveLosses++;
            consecutiveWins = 0;
            AdjustScore(false);
        }

        recentDifficulty[recentDifficultyIndex] = customDifficulty;
        recentDifficultyIndex = (recentDifficultyIndex + 1) % recentDifficultySamples;

        if (IsNearIdealDifficulty())
        {
            AdjustMultiplierNearIdeal();
        }
    }

    private void AdjustScore(bool win)
    {
        if (win)
        {
            customDifficulty += 0.1f * Mathf.Pow(1.2f, consecutiveWins);
        }
        else
        {
            customDifficulty -= 0.1f * Mathf.Pow(1.2f, consecutiveLosses);
        }
        customDifficulty = Mathf.Clamp(customDifficulty, -1, 1);
    }

    private bool IsNearIdealDifficulty()
    {
        float sum = 0;
        for (int i = 0; i < recentDifficultySamples; i++)
        {
            sum += recentDifficulty[i];
        }
        float average = sum / recentDifficultySamples;
        float maxDiff = Mathf.Max(recentDifficulty);
        float minDiff = Mathf.Min(recentDifficulty);
        float range = maxDiff - minDiff;

        return range < difficultyRangeThreshold;
    }

    private void AdjustMultiplierNearIdeal()
    {
        float sum = 0;
        for (int i = 0; i < recentDifficultySamples; i++)
        {
            sum += recentDifficulty[i];
        }
        float average = sum / recentDifficultySamples;
        float diff = Mathf.Abs(average);
        float smoothFactor = Mathf.Clamp01(1.0f - diff * 2.0f);
        difficultyMultiplier *= smoothFactor;
    }
}
