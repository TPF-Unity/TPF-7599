using UnityEngine;

/// <summary>
/// Class that holds data that is persisted between scenes
/// </summary>
public static class GameData
{
    public static int level = 1;
    public static int playerLevel = 1;
    public static float xp = 0f;

    public static void RestartLevel()
    {
        level = 1;
        playerLevel = 1;
        xp = 0f;
    }

    public static void NextLevel()
    {
        level++;
    }
}