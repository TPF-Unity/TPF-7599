using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemySpawner;

    [SerializeField] private float minSpawnDistance = 5f;
    [SerializeField] private float maxSpawnDistance = 100f;

    public void SpawnEnemy() {
        GameObject enemy = Instantiate(enemyPrefab, enemySpawner);
        enemy.transform.parent = enemySpawner;
        enemy.transform.localPosition = Vector3.zero;
    }

    public bool CanSpawn() {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        return minSpawnDistance < distanceToPlayer && distanceToPlayer < maxSpawnDistance;
    }
}
