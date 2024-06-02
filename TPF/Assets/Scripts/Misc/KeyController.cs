using System.Collections.Generic;
using AI.GOAP.Behaviors;
using Misc;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private List<int> collectors = new();


    private readonly List<string> layersThatCanPickKeys = new List<string>()
        { Layer.Player.ToString(), Layer.EnemyPlayers.ToString() };

    private void OnTriggerEnter(Collider other)
    {
        if (layersThatCanPickKeys.Contains(LayerMask.LayerToName(other.gameObject.layer)) && CanPickUpKey(other.gameObject)) {
            KeyCollectorBehavior keyCollector = other.GetComponent<KeyCollectorBehavior>();
            if (keyCollector != null) {
                keyCollector.CollectKey();
            }

            KeyProgressionManager keyManager = other.GetComponent<KeyProgressionManager>();
            if (keyManager != null) {
                keyManager.CollectKey();
            }

            collectors.Add(other.gameObject.GetInstanceID());

            // Only if it was the player who picked up the key
            if (Layer.Player.ToString() == LayerMask.LayerToName(other.gameObject.layer)) {
                GameManager.instance.PickKey();
                if (TryGetComponent<MeshRenderer>(out var meshRenderer)) {
                    meshRenderer.enabled = false;
                }
            }
        }
    }

    public bool CanPickUpKey(GameObject obj) {
        return !collectors.Contains(obj.GetInstanceID());
    }
}