using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PowerUp {
    public PowerUpSO powerUpSO;
    public float duration = 5f;
    public float durationLeft;
    public float floatVal = 2f;

    public PowerUp(PowerUpSO pPowerUpSO, float pDuration, float pFloatVal) {
        powerUpSO = pPowerUpSO;
        duration = pDuration;
        durationLeft = pDuration;
        floatVal = pFloatVal;
    }
}

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private Transform powerUpSpawner;
    [SerializeField] private PowerUpSO powerUpSO;
    [SerializeField] private float powerUpDuration;
    [SerializeField] private float powerUpFloatVal;

    private Transform powerUpPrefab;
    public float respawnTimerMax = 10f;
    private float respawnTimer;
    private bool isActive = true;

    private void Start() {
        powerUpPrefab = Instantiate(powerUpSO.prefab);
        powerUpPrefab.transform.parent = powerUpSpawner;
        powerUpPrefab.transform.localPosition = new Vector3(0, 1, 0);
    }

    private void Update() {
        respawnTimer += Time.deltaTime;
        if (respawnTimer >= respawnTimerMax) {
            respawnTimer = 0f;
            if (!isActive) {
                isActive = true;
                powerUpPrefab.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (ApplyPowerUp(other.gameObject)) {
                isActive = false;
                powerUpPrefab.gameObject.SetActive(false);
                respawnTimer = 0f;
            }
        }
    }

    bool ApplyPowerUp(GameObject playerObject) {
        Player player = playerObject.GetComponent<Player>();
        if (player) {
            PowerUp powerUp = new(powerUpSO, powerUpDuration, powerUpFloatVal);
            return player.PickUpPowerUp(powerUp);
        }
        return false;
    }
}
