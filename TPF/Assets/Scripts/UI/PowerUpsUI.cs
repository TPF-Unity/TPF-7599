using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpsUI : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private Transform iconTemplate;

    private void Start()
    {
        player = MainPlayer.Instance.GetComponent<PlayerController>();
        player.OnPowerUpChanged += Player_OnPowerUpChanged;
    }

    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Player_OnPowerUpChanged(object sender, EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        foreach (Transform child in transform) {
            if (child == iconTemplate) {
                continue;
            }
            Destroy(child.gameObject);
        }

        foreach (PowerUp powerUp in player.GetPowerUpList()) {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.Find("Icon").GetComponent<Image>().sprite = powerUp.powerUpSO.sprite;
        }
    }
}
