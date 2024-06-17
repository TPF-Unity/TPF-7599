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
    }

    public int GetKeysCollected() {
        return collectedKeys;
    }

    public bool HasAllKeys() {
        if (gameManager == null) {
            return false;
        }
        
        requiredKeys = gameManager.getTotalKeys();
        return collectedKeys == requiredKeys;
    }

    public void CollectKey() {
        requiredKeys = gameManager.getTotalKeys();
        collectedKeys ++;
        Debug.Log("Collected keys:" + collectedKeys + "/" + requiredKeys);
        OnKeyCollected?.Invoke();
    }

    public void Reset()
    {
        collectedKeys = 0;
    }
}
