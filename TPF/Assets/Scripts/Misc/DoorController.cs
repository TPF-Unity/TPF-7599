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

    public GameObject portalActive;
    public GameObject portalInactive;


    private void Update() {
        if (open) {
            portalActive.SetActive(true);
            portalInactive.SetActive(false);
        } else {
            portalActive.SetActive(false);
            portalInactive.SetActive(true);
        }
    }

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && open)
        {
            gameManager.Win();
        }
    }

    public void OpenDoor()
    {
        open = true;
    }
}
