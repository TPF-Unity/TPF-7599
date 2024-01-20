using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private bool open = false;

    private Renderer doorRenderer;
    private SceneLoader sceneLoader;

    private void Start () {
        doorRenderer = GetComponent<Renderer> ();
        doorRenderer.material.color = Color.red;
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();

    }

    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag("Player") && open) {
            sceneLoader.LoadGameWinScene();
        }
    }

    public void OpenDoor () {
        open = true;
        doorRenderer.material.color = Color.green;
    }
}
