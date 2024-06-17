using System.Collections;
using System.Collections.Generic;
using Misc;
using StarterAssets;
using TMPro;
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
        KeyProgressionManager keyManager = other.GetComponent<KeyProgressionManager>();
        if (keyManager != null && keyManager.HasAllKeys()) {
            if (other.GetComponent<MainPlayer>() != null) {
                gameManager.Win();
            } else {
                Transform detailText = GameObject.Find("Canvas").transform.Find("GameOverPanel").Find("Container").Find("DetailText");
                TextMeshProUGUI tmpText = detailText.GetComponent<TextMeshProUGUI>();
                tmpText.text = "An enemy player escaped";
                gameManager.Lose();
            }
        }
    }

    public void OpenDoor()
    {
        open = true;
    }
}
