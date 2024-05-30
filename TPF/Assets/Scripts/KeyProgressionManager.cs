using System;
using UnityEngine;

public class KeyProgressionManager : MonoBehaviour
{
    GameManager gameManager;

    public event Action OnKeyCollected;
    
    private int requiredKeys;
    private int collectedKeys;

    void Start() {
        gameManager = GameManager.instance;
        requiredKeys = gameManager.getTotalKeys();
    }

    public int GetKeysCollected() {
        return collectedKeys;
    }

    public bool HasAllKeys() {
        return collectedKeys == requiredKeys;
    }

    public void CollectKey() {
        collectedKeys ++;
        Debug.Log("Collected keys:" + collectedKeys + "/" + requiredKeys);
        OnKeyCollected?.Invoke();
    }
}
