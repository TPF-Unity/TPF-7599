using UnityEngine;

/// <summary>
/// Class that holds data that is persisted between scenes
/// </summary>
public static class GameData
{
    public static int level = 1;

    public static void RestartLevel()
    {
        level = 1;
    }

    public static void NextLevel()
    {
        level++;
    }
}