using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    [SerializeField]
    private bool open = false;

    private Renderer doorRenderer;

    private void Start () {
        doorRenderer = GetComponent<Renderer> ();
        doorRenderer.material.color = Color.red;
    }

    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag ("Player") && open) {
            Debug.Log ("You won!");
        }
    }

    public void OpenDoor () {
        open = true;
        doorRenderer.material.color = Color.green;
    }
}
