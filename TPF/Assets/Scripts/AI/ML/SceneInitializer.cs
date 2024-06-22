using System;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI.ML
{
    public enum MLTrainingScene
    {
        Dungeon2,
        Dungeon3,
        Dungeon4,
        Dungeon5,
        Dungeon6,
        Dungeon7,
        Dungeon8,
        Dungeon9,
        Dungeon10,
        DungeonProcGen
    }

    public class SceneInitializer
    {
        private Dictionary<MLTrainingScene, Action> actionsMap;
        private Dictionary<MLTrainingScene, Vector3[]> startPositionsMap;
        private GameManager _gameManager;
        private Vector3[] _agentStartPositions;
        private Vector3[] _keySpawnPositions;
        private GameObject _agent;
        private EnemySpawnManager _spawnManager;
        private GameObject[] _patrolPoints;

        public void Initialize(GameManager gameManager, Vector3[] startPositions, GameObject agent,
            MLTrainingScene trainingScene)
        {
            var parent = agent.transform.parent?.gameObject;
            _spawnManager = parent?.GetComponentInChildren<EnemySpawnManager>() ??
                            GameObject.Find("SpawnManager").GetComponent<EnemySpawnManager>();
            _agent = agent;
            _gameManager = gameManager;
            _keySpawnPositions = new Vector3[_gameManager.keySpawnPositions.Length];
            _patrolPoints = GameObject.FindGameObjectsWithTag(Tags.PatrolPoint.ToString());

            for (int i = 0; i < _gameManager.keySpawnPositions.Length; i++)
            {
                _keySpawnPositions[i] = _gameManager.keySpawnPositions[i].transform.position;
            }

            actionsMap = new Dictionary<MLTrainingScene, Action>();
            startPositionsMap = new Dictionary<MLTrainingScene, Vector3[]>();
            actionsMap.Add(MLTrainingScene.Dungeon2, InitializeDungeon2);
            actionsMap.Add(MLTrainingScene.Dungeon3, InitializeDungeon3);
            actionsMap.Add(MLTrainingScene.Dungeon4, InitializeDungeon4);
            actionsMap.Add(MLTrainingScene.Dungeon5, InitializeDungeon5);
            actionsMap.Add(MLTrainingScene.Dungeon6, InitializeDungeon6);
            actionsMap.Add(MLTrainingScene.Dungeon7, InitializeDungeon7);
            actionsMap.Add(MLTrainingScene.Dungeon8, InitializeDungeon8);
            actionsMap.Add(MLTrainingScene.Dungeon9, InitializeDungeon9);
            actionsMap.Add(MLTrainingScene.Dungeon10, InitializeDungeon10);
            actionsMap.Add(MLTrainingScene.DungeonProcGen, InitializeDungeonProcGen);

            startPositionsMap.Add(MLTrainingScene.Dungeon2, new[] { startPositions[0] });
            startPositionsMap.Add(MLTrainingScene.Dungeon3, new[] { startPositions[0] });
            startPositionsMap.Add(MLTrainingScene.Dungeon4, new[] { startPositions[0] });
            startPositionsMap.Add(MLTrainingScene.Dungeon5, new[] { startPositions[0] });
            startPositionsMap.Add(MLTrainingScene.Dungeon6, startPositions);
            startPositionsMap.Add(MLTrainingScene.Dungeon7, new[] { startPositions[0] });
            startPositionsMap.Add(MLTrainingScene.Dungeon8, startPositions);
            startPositionsMap.Add(MLTrainingScene.Dungeon9, startPositions);
            startPositionsMap.Add(MLTrainingScene.Dungeon10, startPositions);
            startPositionsMap.Add(MLTrainingScene.DungeonProcGen, startPositions);
        }

        public void Restart(MLTrainingScene scene)
        {
            actionsMap.TryGetValue(scene, out var restart);
            restart?.Invoke();
        }

        private void ResetKeys(float distanceFromInitialPosition)
        {
            for (var i = 0; i < _gameManager.keySpawnPositions.Length; i++)
            {
                var key = _gameManager.keySpawnPositions[i];
                key.gameObject.SetActive(true);
                key.transform.position = _keySpawnPositions[i] +
                                         Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) *
                                         Vector3.forward * 1f;
            }

            var mlKeyFlags = GameObject.FindGameObjectsWithTag("KeyFlagML");
            foreach (var mlKeyFlag in mlKeyFlags)
            {
                mlKeyFlag.GetComponent<Collider>().enabled = true;
            }
        }


        private void InitializeDungeon2()
        {
            startPositionsMap.TryGetValue(MLTrainingScene.Dungeon2, out var startPositions);
            _agent.transform.position = startPositions[Random.Range(0, startPositions.Length - 1)];
            foreach (var key in _gameManager.keys)
            {
                key.gameObject.SetActive(true);
                key.transform.position = startPositions[0] +
                                         Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) *
                                         Vector3.forward * 8f;
                _patrolPoints[0].transform.position = key.transform.position +
                                                      Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) *
                                                      Vector3.forward * 2f;
                _gameManager._doors[0].transform.position = startPositions[0] +
                                                            Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) *
                                                            Vector3.forward * 2f;
                foreach (var gameManagerKey in _gameManager.keys)
                {
                    var keyController = gameManagerKey.GetComponentInChildren<KeyController>();
                    keyController.Reset();
                }
            }

            var mlKeyFlags = GameObject.FindGameObjectsWithTag("KeyFlagML");
            Debug.Log(mlKeyFlags.Length);
            foreach (var mlKeyFlag in mlKeyFlags)
            {
                mlKeyFlag.GetComponent<Collider>().enabled = true;
            }
        }

        private void InitializeDungeon3()
        {
            startPositionsMap.TryGetValue(MLTrainingScene.Dungeon2, out var startPositions);
            _agent.transform.position = startPositions[Random.Range(0, startPositions.Length - 1)];

            for (var i = 0; i < _gameManager.keys.Length; i++)
            {
                var key = _gameManager.keys[i];
                key.gameObject.SetActive(true);
                key.transform.position = _patrolPoints[i].transform.position +
                                         Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) *
                                         Vector3.forward * 1f;
            }

            for (var i = 0; i < _gameManager._doors.Length; i++)
            {
                var door = _gameManager._doors[i];
                door.transform.position = _patrolPoints[i].transform.position +
                                          Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) *
                                          Vector3.forward * 1f;
            }

            foreach (var gameManagerKey in _gameManager.keys)
            {
                var keyController = gameManagerKey.GetComponentInChildren<KeyController>();
                keyController.Reset();
            }

            var mlKeyFlags = GameObject.FindGameObjectsWithTag("KeyFlagML");
            Debug.Log(mlKeyFlags.Length);
            foreach (var mlKeyFlag in mlKeyFlags)
            {
                mlKeyFlag.GetComponent<Collider>().enabled = true;
            }
        }

        private void InitializeDungeon4()
        {
            Vector3 offset = new Vector3(-12, 0, 9);
            for (var i = 0; i < _gameManager.keySpawnPositions.Length; i++)
            {
                var key = _gameManager.keySpawnPositions[i];
                key.transform.position = _keySpawnPositions[i] + Random.Range(0, 2) * offset;
                key.gameObject.SetActive(true);
            }
        }

        private void InitializeDungeon5()
        {
            ResetKeys(2f);
        }

        private void InitializeDungeon6()
        {
            startPositionsMap.TryGetValue(MLTrainingScene.Dungeon6, out var startPositions);
            _agent.transform.position = startPositions[Random.Range(0, startPositions.Length - 1)];

            ResetKeys(2f);
        }

        private void InitializeDungeon7()
        {
            startPositionsMap.TryGetValue(MLTrainingScene.Dungeon7, out var startPositions);
            _agent.transform.position = startPositions[Random.Range(0, startPositions.Length - 1)];
            ResetKeys(1f);
        }

        private void InitializeDungeon8()
        {
            startPositionsMap.TryGetValue(MLTrainingScene.Dungeon8, out var startPositions);
            _agent.transform.position = startPositions[Random.Range(0, startPositions.Length - 1)];

            var shuffledPatrolPoints = _patrolPoints.ShuffledCopy();
            for (var i = 0; i < _gameManager.keys.Length; i++)
            {
                var key = _gameManager.keys[i];
                key.gameObject.SetActive(true);
                key.transform.position = shuffledPatrolPoints[i].transform.position +
                                         Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) *
                                         Vector3.forward * 1f;
            }

            shuffledPatrolPoints = _patrolPoints.ShuffledCopy();
            for (var i = 0; i < _gameManager._doors.Length; i++)
            {
                var door = _gameManager._doors[i];
                door.transform.position = shuffledPatrolPoints[i].transform.position +
                                          Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) *
                                          Vector3.forward * 1f;
            }

            foreach (var gameManagerKey in _gameManager.keys)
            {
                var keyController = gameManagerKey.GetComponentInChildren<KeyController>();
                keyController.Reset();
            }

            var mlKeyFlags = GameObject.FindGameObjectsWithTag("KeyFlagML");
            Debug.Log(mlKeyFlags.Length);
            foreach (var mlKeyFlag in mlKeyFlags)
            {
                mlKeyFlag.GetComponent<Collider>().enabled = true;
            }
        }

        private void InitializeDungeonProcGen()
        {
            startPositionsMap.TryGetValue(MLTrainingScene.Dungeon8, out var startPositions);
            _agent.transform.position = startPositions[Random.Range(0, startPositions.Length - 1)] + new Vector3(0, 1, 0);

            var shuffledPatrolPoints = _patrolPoints.ShuffledCopy();
            for (var i = 0; i < _gameManager.keys.Length; i++)
            {
                var key = _gameManager.keys[i];
                key.gameObject.SetActive(true);
                key.transform.position = shuffledPatrolPoints[i].transform.position +
                                         Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) *
                                         Vector3.forward * 1f;
            }

            shuffledPatrolPoints = _patrolPoints.ShuffledCopy();
            for (var i = 0; i < _gameManager._doors.Length; i++)
            {
                var door = _gameManager._doors[i];
                door.transform.position = shuffledPatrolPoints[i].transform.position +
                                          Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)) *
                                          Vector3.forward * 1f;
            }

            foreach (var gameManagerKey in _gameManager.keys)
            {
                var keyController = gameManagerKey.GetComponentInChildren<KeyController>();
                keyController.Reset();
            }

            var mlKeyFlags = GameObject.FindGameObjectsWithTag("KeyFlagML");
            Debug.Log(mlKeyFlags.Length);
            foreach (var mlKeyFlag in mlKeyFlags)
            {
                mlKeyFlag.GetComponent<Collider>().enabled = true;
            }
            
            _spawnManager.DeSpawnAllEnemies();
        }

        private void InitializeDungeon9()
        {
            startPositionsMap.TryGetValue(MLTrainingScene.Dungeon9, out var startPositions);
            _agent.transform.position = startPositions[Random.Range(0, startPositions.Length)];
            ResetKeys(1f);
        }

        private void InitializeDungeon10()
        {
            startPositionsMap.TryGetValue(MLTrainingScene.Dungeon10, out var startPositions);
            _agent.transform.position = startPositions[Random.Range(0, startPositions.Length)];
            ResetKeys(1f);

            EnemySpawnManager.Instance.DeSpawnAllEnemies();
            _spawnManager.DeSpawnAllEnemies();
        }
    }
}