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
    private DoorController[] _doors;
    public GameObject keyPrefab;
    public GameObject doorPrefab;

    private int _totalKeys;
    private int _totalDoors;

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

        for (int i = 0; i < _totalKeys; i++)
        {
            Instantiate(keyPrefab, spawnPositions[i].transform.position, Quaternion.identity);
        }
    }

    private void SpawnDoors()
    {
        GameObject[] spawnPositions = GameObject.FindGameObjectsWithTag("DoorSpawn");
        _totalDoors = spawnPositions.Length;

        for (int i = 0; i < _totalDoors; i++)
        {
            GameObject door = Instantiate(doorPrefab, spawnPositions[i].transform.position, Quaternion.identity);
            _doors = _doors.Concat(new DoorController[] { door.GetComponent<DoorController>() }).ToArray();
        }
    }

    public int getRecolectedKeys()
    {
        return _recolectedKeys;
    }

    public int getTotalKeys()
    {
        return _totalKeys;
    }
}
