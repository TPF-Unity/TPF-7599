using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private readonly List<string> layersThatCanPickKeys = new List<string>() { "Player", "EnemyPlayers" };

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (layersThatCanPickKeys.Contains(LayerMask.LayerToName(other.gameObject.layer)))
        {
            GameManager.instance.PickKey();
            gameObject.SetActive(false);
        }
    }
}