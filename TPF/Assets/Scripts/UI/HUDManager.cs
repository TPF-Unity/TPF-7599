using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI keysText;
    public GameManager gameManager;

    void Awake()
    {
        gameManager = GetComponentInParent<GameManager>();
    }

    void Update()
    {
        if (gameManager != null)
        {
            keysText.text = gameManager.getRecolectedKeys() + "/" + gameManager.getTotalKeys();
        }
    }
}