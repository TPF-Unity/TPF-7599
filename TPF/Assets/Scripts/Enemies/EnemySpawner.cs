using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemySpawner;

    private float minSpawnDistance = 15f;
    private GameObject[] spawnPoints;

    private void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");
    }

    public void SpawnEnemy() {

        List<GameObject> eligibleSpawnPoints = new List<GameObject>();
        foreach (GameObject point in spawnPoints)
        {
            if (CanSpawn(point.transform.position))
            {
                eligibleSpawnPoints.Add(point);
            }
        }

        if (eligibleSpawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, eligibleSpawnPoints.Count);
            GameObject chosenPoint = eligibleSpawnPoints[randomIndex];

            GameObject enemy = Instantiate(enemyPrefab, chosenPoint.transform.position, Quaternion.identity);
            enemy.transform.parent = enemySpawner;
            enemy.transform.localPosition = Vector3.zero;
        }
    }

    public bool CanSpawn(Vector3 position) {
        float distanceToPlayer = Vector3.Distance(position, Player.Instance.transform.position);
        return minSpawnDistance < distanceToPlayer;
    }
}
