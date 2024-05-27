using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private bool open = false;

    private Renderer doorRenderer;

    private GameManager gameManager;


    private void Start()
    {
        doorRenderer = GetComponent<Renderer>();
        doorRenderer.material.color = Color.red;
        gameManager = GameManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        KeyProgressionManager keyManager = other.GetComponent<KeyProgressionManager>();
        if (keyManager != null && keyManager.HasAllKeys()) {
            if (other.CompareTag("Player")) {
                gameManager.Win();
            } else {
                gameManager.Lose();
            }
        }
    }

    public void OpenDoor()
    {
        open = true;
        doorRenderer.material.color = Color.green;
    }
}
