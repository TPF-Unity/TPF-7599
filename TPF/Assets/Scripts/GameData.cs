using UnityEngine;

/// <summary>
/// Class that holds data that is persisted between scenes
/// </summary>
public static class GameData
{
    public static int level = 1;
    public static int consecutiveWins = 0;
    public static int consecutiveLosses = 0;
    public static float customDifficulty;
    public static float difficultyMultiplier = 1.0f;
    public static int recentDifficultySamples = 5;
    public static float[] recentDifficulty = new float[recentDifficultySamples];
    public static int recentDifficultyIndex = 0;
    public static float difficultyRangeThreshold = 0.3f;

    public static void RestartLevel()
    {
        level = 1;
    }

    public static void NextLevel()
    {
        level++;
    }
}