using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField]
    int consecutiveWins = 0;
    [SerializeField]
    int consecutiveLosses = 0;

    private void Update()
    {
        consecutiveLosses = GameData.consecutiveLosses;
        consecutiveWins = GameData.consecutiveWins;
    }

    public void SetDifficulty(string difficulty)
    {
        PlayerPrefs.SetString("Difficulty", difficulty);
    }

    public void SetCustomDifficulty(float customDifficulty)
    {
        GameData.customDifficulty = customDifficulty;
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
                return GameData.customDifficulty;
            default:
                return 0;
        }
    }

    public void MatchResult(bool win)
    {
        if (PlayerPrefs.GetString("Difficulty", "NORMAL") != "CUSTOM") return;
        if (win)
        {
            GameData.consecutiveWins++;
            GameData.consecutiveLosses = 0;
            AdjustScore(true);
        }
        else
        {
            GameData.consecutiveLosses++;
            GameData.consecutiveWins = 0;
            AdjustScore(false);
        }

        GameData.recentDifficulty[GameData.recentDifficultyIndex] = GameData.customDifficulty;
        GameData.recentDifficultyIndex = (GameData.recentDifficultyIndex + 1) % GameData.recentDifficultySamples;

        if (IsNearIdealDifficulty())
        {
            AdjustMultiplierNearIdeal();
        }
    }

    private void AdjustScore(bool win)
    {
        if (win)
        {
            GameData.customDifficulty += 0.1f * Mathf.Pow(1.2f, GameData.consecutiveWins);
        }
        else
        {
            GameData.customDifficulty -= 0.1f * Mathf.Pow(1.2f, GameData.consecutiveLosses);
        }
        GameData.customDifficulty = Mathf.Clamp(GameData.customDifficulty, -1, 1);
    }

    private bool IsNearIdealDifficulty()
    {
        float sum = 0;
        for (int i = 0; i < GameData.recentDifficultySamples; i++)
        {
            sum += GameData.recentDifficulty[i];
        }
        float average = sum / GameData.recentDifficultySamples;
        float maxDiff = Mathf.Max(GameData.recentDifficulty);
        float minDiff = Mathf.Min(GameData.recentDifficulty);
        float range = maxDiff - minDiff;

        return range < GameData.difficultyRangeThreshold;
    }

    private void AdjustMultiplierNearIdeal()
    {
        float sum = 0;
        for (int i = 0; i < GameData.recentDifficultySamples; i++)
        {
            sum += GameData.recentDifficulty[i];
        }
        float average = sum / GameData.recentDifficultySamples;
        float diff = Mathf.Abs(average);
        float smoothFactor = Mathf.Clamp01(1.0f - diff * 2.0f);
        GameData.difficultyMultiplier *= smoothFactor;
    }
}
