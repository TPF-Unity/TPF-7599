using UnityEngine;

[DefaultExecutionOrder(10000)] // Ensures UI initializes last
public class ConfigManager : MonoBehaviour
{
    public void SetDifficulty(string difficulty)
    {
        PlayerPrefs.SetString("Difficulty", difficulty);
        Debug.Log("Difficulty set to " + PlayerPrefs.GetString("Difficulty", "MISSING"));
    }

    public void SetAIStrategy(string strat)
    {
        GameManager.instance.UseGOAP = strat == "GOAP";
    }

}
