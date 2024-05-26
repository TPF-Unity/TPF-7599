using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    public TextMeshProUGUI keysText;
    public TextMeshProUGUI levelText;

    void Update()
    {
        keysText.text = GameManager.instance.getRecolectedKeys() + "/" + GameManager.instance.getTotalKeys();
        levelText.text = "Level " + GameData.level.ToString();
    }
}
