using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class XPUI : MonoBehaviour
{

    private PlayerController player;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI levelText;


    private void Start()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var currentPlayer in players)
        {
            var playerController = currentPlayer.GetComponent<PlayerController>();
            if (playerController != null)
            {
                player = playerController;
            }
        }

        player.onXPChanged.AddListener(Player_OnXPGained);
        UpdateVisual(0);
    }

    private void Player_OnXPGained(float xp) {
        UpdateVisual(xp);
    }

    private void UpdateVisual(float xp) {
        int level = player.GetLV();
        slider.value = xp;
        levelText.text = "LV" + level.ToString();
    }

}