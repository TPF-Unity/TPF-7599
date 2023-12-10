using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance { get; private set; }

    [SerializeField]
    private int enemyAmountMax = 2;
    private int enemyAmount = 0;

    [SerializeField]
    private float enemyTimerMax = 5f;
    private float enemyTimer = 0f;

    [SerializeField]
    private List<EnemySpawner> enemySpawnerList;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Unit.OnDestroyed += HandleDeath;
    }

    private void HandleDeath(object sender, EventArgs e) {
        enemyAmount --;
        if (enemyAmount < 0) {
            enemyAmount = 0;
        }
    }

    private void Update() {
        enemyTimer -= Time.deltaTime;

        if (enemyTimer < 0f) {
            enemyTimer = enemyTimerMax;
            if (enemyAmount < enemyAmountMax) {
                // spawn enemy
                List<EnemySpawner> availableSpawners = new();
                foreach (EnemySpawner spawner in enemySpawnerList) {
                    if (spawner.CanSpawn()) {
                        availableSpawners.Add(spawner);
                    }
                }
                if (availableSpawners.Count > 0) {
                    EnemySpawner spawner = availableSpawners[UnityEngine.Random.Range(0, availableSpawners.Count)];
                    spawner.SpawnEnemy();
                    enemyAmount ++;
                }
            }
        }
    }
}
