using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private bool open = false;

    private Renderer doorRenderer;

    private GameManager gameManager;


    private void Start()
    {
        doorRenderer = GetComponent<Renderer>();
        doorRenderer.material.color = Color.red;
        gameManager = GameManager.instance;
        OpenDoor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && open)
        {
            var player = other.GetComponent<Player>();
            if (player.isMainPlayer)
            {
                gameManager.Win();
            }
        }
    }

    public void OpenDoor()
    {
        open = true;
        doorRenderer.material.color = Color.green;
    }
}