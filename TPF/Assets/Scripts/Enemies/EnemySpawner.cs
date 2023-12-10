using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Transform enemySpawner;

    private float minSpawnDistance = 5f;
    private float maxSpawnDistance = 20f;

    public void SpawnEnemy() {
        Transform enemy = Instantiate(enemyPrefab, enemySpawner);
        enemy.parent = enemySpawner;
        enemy.localPosition = Vector3.zero;
    }

    public bool CanSpawn() {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        return minSpawnDistance < distanceToPlayer && distanceToPlayer < maxSpawnDistance;
    }
}
