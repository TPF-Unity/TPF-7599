using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Misc;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemySpawner;

    private float minSpawnDistance = 15f;
    private GameObject[] spawnPoints;
    private List<GameObject> enemies = new();

    private void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");
    }

    public void SpawnEnemy()
    {
        if (spawnPoints == null)
        {
            spawnPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");
        }

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
            enemies.Add(enemy);
        }
    }

    public bool CanSpawn(Vector3 position)
    {
        var colliders = Physics.OverlapSphere(position, minSpawnDistance,
            LayerMask.NameToLayer(Layer.Player.ToString()) | LayerMask.NameToLayer(Layer.EnemyPlayers.ToString()));
        var playerNearby = colliders.Any();
        return !playerNearby;
    }

    public void DeSpawnAllEnemies()
    {
        enemies.ForEach(Destroy);
        enemies = new List<GameObject>();
    }
}