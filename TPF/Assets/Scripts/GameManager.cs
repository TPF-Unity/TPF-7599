using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Misc;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private int _recolectedKeys = 0;
    [SerializeField] private int _totalKeys = 3;
    [SerializeField] private int _requiredKeys = 1;
    [SerializeField] private int _totalDoors = 2;
    [SerializeField] private DoorController[] _doors;
    public GameObject keyPrefab;
    public GameObject doorPrefab;
    [SerializeField] public GameObject[] keySpawnPositions;
    [SerializeField] public GameObject[] doorSpawnPositions;

    public DifficultyManager difficultyManager;
    private SceneLoader sceneLoader;
    private List<GameObject> _keys;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void InitializeScene()
    {
        SpawnKeys();
        SpawnDoors();
    }

    private void Start()
    {
        difficultyManager = GetComponent<DifficultyManager>();
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        InitializeScene();
    }

    public void PickKey(Player player)
    {
        player.KeysCollected++;
        if (player.KeysCollected >= _requiredKeys)
        {
            foreach (var t in _doors)
            {
                t.OpenDoor();
            }
        }

        _recolectedKeys++;
    }

    private void RemoveKeys()
    {
        if (_keys != null)
        {
            Debug.Log("Destroying key");
            _keys.ForEach(Destroy);
        }

        _recolectedKeys = 0;
        _keys = new List<GameObject>();
    }

    private void RemoveDoors()
    {
        foreach (var doorController in _doors)
        {
            Destroy(doorController.gameObject);
        }

        _doors = Array.Empty<DoorController>();
    }

    private void SpawnKeys()
    {
        Debug.Log("Spawning Keys");
        RemoveKeys();

        //Transform player = GameObject.FindGameObjectWithTag(Tags.Player.ToString()).transform;
        int[] randomIndex = Enumerable.Range(0, keySpawnPositions.Length)
            .OrderBy(x => Random.Range(0, keySpawnPositions.Length)).Take(_totalKeys).ToArray();
        for (int i = 0; i < _totalKeys; i++)
        {
            int index = randomIndex[i];
            var key = Instantiate(keyPrefab, keySpawnPositions[index].transform.position, Quaternion.identity);
            _keys.Add((key));
        }
    }

    private void SpawnDoors()
    {
        RemoveDoors();
        //Transform player = GameObject.FindGameObjectWithTag(Tags.Player.ToString()).transform;
        int[] randomIndex = Enumerable.Range(0, doorSpawnPositions.Length)
            .OrderBy(x => Random.Range(0, doorSpawnPositions.Length)).Take(_totalDoors).ToArray();
        for (int i = 0; i < _totalDoors; i++)
        {
            int index = randomIndex[i];
            GameObject door = Instantiate(doorPrefab, doorSpawnPositions[index].transform.position,
                Quaternion.identity);
            _doors = _doors.Concat(new DoorController[] { door.GetComponent<DoorController>() }).ToArray();
        }
    }

    public void Win()
    {
        sceneLoader.LoadGameWinScene();
        difficultyManager.MatchResult(true);
    }

    public void Lose()
    {
        difficultyManager.MatchResult(false);
    }

    public int getRecolectedKeys()
    {
        return _recolectedKeys;
    }

    public int getTotalKeys()
    {
        return _totalKeys;
    }

    public float GetDifficulty()
    {
        return difficultyManager.GetDifficulty();
    }

    public bool isDoor(GameObject gameObj)
    {
        foreach (var door in _doors)
        {
            if (door.gameObject == gameObj)
            {
                return true;
            }
        }

        return false;
    }
}