using System.Collections;
using System.Collections.Generic;
using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    public TextMeshProUGUI keysText;
    public TextMeshProUGUI levelText;
    public GameManager gameManager;

    void Awake()
    {
        gameManager = GameObject.Find(GameObjects.GameManager.ToString()).GetComponent<GameManager>();
    }

    void Update()
    {
        keysText.text = gameManager.getRecolectedKeys() + "/" + gameManager.getTotalKeys();
        if (levelText)
        {
            levelText.text = "Level " + GameData.level.ToString();
        }
    }
}
