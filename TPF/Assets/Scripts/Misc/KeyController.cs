using System;
using System.Collections.Generic;
using AI.GOAP.Behaviors;
using Misc;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private GameManager _gameManager;

    private List<string> _layersThatCanPickKeys = new()
        { Layer.Player.ToString(), Layer.EnemyPlayers.ToString() };

    private void Awake()
    {
        if (_gameManager == null)
        {
            _gameManager = GameObject.Find(GameObjects.GameManager.ToString()).GetComponent<GameManager>();
        }

        _layersThatCanPickKeys = new List<string> { Layer.Player.ToString(), Layer.EnemyPlayers.ToString() };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_layersThatCanPickKeys.Contains(LayerMask.LayerToName(other.gameObject.layer)))
        {
            _gameManager.PickKey(other.gameObject.GetComponent<Player>());
            var keyCollector = other.GetComponent<KeyCollectorBehavior>();
            if (keyCollector != null)
            {
                keyCollector.CollectKey();
            }

            gameObject.SetActive(false);
        }
    }
}