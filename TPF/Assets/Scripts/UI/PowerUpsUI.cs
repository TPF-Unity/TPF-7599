using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpsUI : MonoBehaviour
{
    private Player[] _players;
    [SerializeField] private Transform iconTemplate;

    private void Start()
    {
        var playerObjects = GameObject.FindGameObjectsWithTag("Player");
        _players = playerObjects.Select(player => player.GetComponent<Player>())
            .ToArray();
        foreach (var player in _players)
        {
            player.GetComponent<Player>().OnPowerUpChanged += Player_OnPowerUpChanged;
        }
    }

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Player_OnPowerUpChanged(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate)
            {
                continue;
            }

            Destroy(child.gameObject);
        }

        foreach (var player in _players)
        {
            foreach (PowerUp powerUp in player.GetPowerUpList())
            {
                Transform iconTransform = Instantiate(iconTemplate, transform);
                iconTransform.gameObject.SetActive(true);
                iconTransform.Find("Icon").GetComponent<Image>().sprite = powerUp.powerUpSO.sprite;
            }
        }
    }
}