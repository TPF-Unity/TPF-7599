using System.Collections.Generic;
using AI.GOAP.Behaviors;
using Misc;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private readonly List<string> layersThatCanPickKeys = new List<string>()
        { Layer.Player.ToString(), Layer.EnemyPlayers.ToString() };

    private void OnTriggerEnter(Collider other)
    {
        if (layersThatCanPickKeys.Contains(LayerMask.LayerToName(other.gameObject.layer)))
        {
            GameManager.instance.PickKey();
            KeyCollectorBehavior keyCollector = other.GetComponent<KeyCollectorBehavior>();
            if (keyCollector != null)
            {
                keyCollector.CollectKey();
            }

            gameObject.SetActive(false);
        }
    }
}