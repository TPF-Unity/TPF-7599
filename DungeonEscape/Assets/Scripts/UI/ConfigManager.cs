using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(10000)] // Ensures UI initializes last
public class ConfigManager : MonoBehaviour
{
    [SerializeField] private Button easyButton;
    [SerializeField] private Button normalButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Button customButton;
    [SerializeField] private Button goapButton;
    [SerializeField] private Button btButton;
    [SerializeField] private Button mlButton;

    private Color defaultColor = Color.white;
    private Color selectedColor = new(0.5f, 0.5f, 0.5f);

    private void Start()
    {
        SetButtonColor(normalButton, selectedColor);
        SetButtonColor(goapButton, selectedColor);
        PlayerPrefs.SetString("OpponentAI", "GOAP");
    }

    public void SetDifficulty(string difficulty)
    {
        SetButtonColor(easyButton, defaultColor);
        SetButtonColor(normalButton, defaultColor);
        SetButtonColor(hardButton, defaultColor);
        SetButtonColor(customButton, defaultColor);

        Button btn = null;
        switch (difficulty) {
            case "EASY":
                btn = easyButton;
                break;
            case "NORMAL":
                btn = normalButton;
                break;
            case "HARD":
                btn = hardButton;
                break;
            case "CUSTOM":
                btn = customButton;
                break;
            default:
                break;
        }
        SetButtonColor(btn, selectedColor);

        PlayerPrefs.SetString("Difficulty", difficulty);
        Debug.Log("Difficulty set to " + PlayerPrefs.GetString("Difficulty", "MISSING"));
    }

    public void SetAIStrategy(string strat)
    {
        SetButtonColor(goapButton, defaultColor);
        SetButtonColor(btButton, defaultColor);
        SetButtonColor(mlButton, defaultColor);
        
        Button btn = null;
        switch (strat) {
            case "GOAP":
                btn = goapButton;
                break;
            case "BT":
                btn = btButton;
                break;
            case "ML":
                btn = mlButton;
                break;
            default:
                break;
        }
        SetButtonColor(btn, selectedColor);

        PlayerPrefs.SetString("OpponentAI", strat);
        Debug.Log("Opponent AI strategy set to " + PlayerPrefs.GetString("OpponentAI", "MISSING"));
    }

    private void SetButtonColor(Button btn, Color color)
    {
        Image btnImage = btn.GetComponent<Image>();
        btnImage.color = color;
    }

}
