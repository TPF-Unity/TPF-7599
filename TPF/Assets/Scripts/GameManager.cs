using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    private int _recolectedKeys = 0;
    [SerializeField]
    private int _totalKeys = 3;
    [SerializeField]
    private int _totalDoors = 2;
    [SerializeField]
    private DoorController[] _doors;
    public GameObject keyPrefab;
    public GameObject doorPrefab;
    [SerializeField]
    public GameObject[] keySpawnPositions;
    [SerializeField]
    public GameObject[] doorSpawnPositions;

    public DifficultyManager difficultyManager;
    private SceneLoader sceneLoader;

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

    private void Start()
    {
        difficultyManager = GetComponent<DifficultyManager>();
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
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
        _recolectedKeys = 0;
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        int[] randomIndex = Enumerable.Range(0, keySpawnPositions.Length).OrderBy(x => Random.Range(0, keySpawnPositions.Length)).Take(_totalKeys).ToArray();
        for (int i = 0; i < _totalKeys; i++)
        {
            int index = randomIndex[i];
            Instantiate(keyPrefab, keySpawnPositions[index].transform.position, Quaternion.identity);
        }
    }

    private void SpawnDoors()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        int[] randomIndex = Enumerable.Range(0, doorSpawnPositions.Length).OrderBy(x => Random.Range(0, doorSpawnPositions.Length)).Take(_totalDoors).ToArray();
        for (int i = 0; i < _totalDoors; i++)
        {
            int index = randomIndex[i];
            GameObject door = Instantiate(doorPrefab, doorSpawnPositions[index].transform.position, Quaternion.identity);
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

}
