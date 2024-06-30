using System.Collections.Generic;
using AI.GOAP.Behaviors;
using Misc;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private List<int> collectors = new();
    
    public delegate void KeyCollectedEvent(GameObject key);

    public event KeyCollectedEvent OnKeyCollected;

    private readonly List<string> layersThatCanPickKeys = new List<string>()
        { Layer.Player.ToString(), Layer.EnemyPlayers.ToString() };

    private readonly List<string> layersThatDoSoundWhenPickingKeys = new List<string>()
        { Layer.Player.ToString() };

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

            if (layersThatDoSoundWhenPickingKeys.Contains(LayerMask.LayerToName(other.gameObject.layer))) {
                AudioSource.PlayClipAtPoint(GameManager.instance.keySound, transform.position, GameManager.instance.effectsVolume);
            }

            collectors.Add(other.gameObject.GetInstanceID());
            OnKeyCollected?.Invoke(gameObject);


            // Only if it was the player who picked up the key
            if (Layer.Player.ToString() == LayerMask.LayerToName(other.gameObject.layer)) {
                GameManager.instance.PickKey();
                if (TryGetComponent<MeshRenderer>(out var meshRenderer) && !GameManager.instance.isTraining) {
                    meshRenderer.enabled = false;
                }
            }
        }
    }
    

    public bool CanPickUpKey(GameObject obj) {
        return !collectors.Contains(obj.GetInstanceID());
    }

    public void Reset()
    {
        collectors = new List<int>();
        GetComponent<MeshRenderer>().enabled = true;
    }
}