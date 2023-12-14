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

    private void Start()
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
        _recolectedKeys = 0;
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < _totalKeys; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-5, 5), 0.1f, Random.Range(-5, 5));
            Instantiate(keyPrefab, player.position + randomPosition, Quaternion.identity);
        }
    }

    private void SpawnDoors()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < _totalDoors; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-10, 10), 0.5f, Random.Range(-10, 10));
            GameObject door = Instantiate(doorPrefab, player.position + randomPosition, Quaternion.identity);
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
