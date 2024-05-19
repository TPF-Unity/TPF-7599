using System.Collections.Generic;
using AI.GOAP.Behaviors;
using Misc;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private List<string> _layersThatCanPickKeys = new()
        { Layer.Player.ToString(), Layer.EnemyPlayers.ToString() };

    private void Awake()
    {
        _layersThatCanPickKeys = new List<string> { Layer.Player.ToString(), Layer.EnemyPlayers.ToString() };
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_layersThatCanPickKeys.Contains(LayerMask.LayerToName(other.gameObject.layer)))
        {
            GetComponentInParent<GameManager>().PickKey();
            KeyCollectorBehavior keyCollector = other.GetComponent<KeyCollectorBehavior>();
            if (keyCollector != null)
            {
                keyCollector.CollectKey();
            }

            gameObject.SetActive(false);
        }
    }
}