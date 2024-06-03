using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Misc;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private int _recolectedKeys = 0;
    [SerializeField] public DoorController[] _doors;
    public GameObject keyPrefab;
    public GameObject doorPrefab;

    private int _totalKeys;
    private int _totalDoors;

    public GameObject[] keySpawnPositions;
    public GameObject[] doorSpawnPositions;

    public GameObject[] keys;
    
    public event System.Action<bool> OnStrategyChange;
    public bool isTraining = false;
    [SerializeField] private bool useGOAP;

    public bool UseGOAP
    {
        get { return useGOAP; }
        set
        {
            if (useGOAP != value)
            {
                useGOAP = value;
                OnStrategyChange?.Invoke(useGOAP);
            }
        }
    }

    public DifficultyManager difficultyManager;
    private SceneLoader sceneLoader;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Initialize()
    {
        difficultyManager = GetComponent<DifficultyManager>();
        sceneLoader = GameObject.Find(GameObjects.SceneLoader.ToString()).GetComponent<SceneLoader>();
        SpawnKeys();
        SpawnDoors();
    }

    public void PickKey()
    {
        _recolectedKeys++;
        if (_recolectedKeys >= _totalKeys)
        {
            for (int i = 0; i < _doors.Length; i++)
            {
                _doors[i].OpenDoor();
            }
        }
    }

    private void SpawnKeys()
    {
        GameObject[] spawnPositions = GameObject.FindGameObjectsWithTag("KeySpawn");
        _recolectedKeys = 0;
        _totalKeys = spawnPositions.Length;
        keySpawnPositions = new GameObject[_totalKeys];
        keys = new GameObject[_totalKeys];
        for (int i = 0; i < _totalKeys; i++)
        {
            var key = Instantiate(keyPrefab, spawnPositions[i].transform.position, Quaternion.identity);
            keySpawnPositions[i] = key;
            keys[i] = key;
        }
    }

    private void SpawnDoors()
    {
        GameObject[] spawnPositions = GameObject.FindGameObjectsWithTag("DoorSpawn");
        _totalDoors = spawnPositions.Length;
        _doors = new DoorController[_totalDoors];
        doorSpawnPositions = new GameObject[_totalDoors];
        
        for (int i = 0; i < _totalDoors; i++)
        {
            GameObject door = Instantiate(doorPrefab, spawnPositions[i].transform.position, Quaternion.identity);
            doorSpawnPositions[i] = door;
            _doors[i] = door.GetComponent<DoorController>();
        }
    }

    public void Win()
    {
        if (isTraining)
        {
            return;
        }

        difficultyManager.MatchResult(true);
        GameData.NextLevel();
        sceneLoader.LoadMainScene();
    }

    public void Lose()
    {
        sceneLoader.LoadGameLoseScene();
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
}