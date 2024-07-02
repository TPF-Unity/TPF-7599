using System;
using System.Collections;
using System.Collections.Generic;
using AI.GOAP.Behaviors;
using AI.ML;
using CrashKonijn.Goap.Behaviours;
using UnityEngine;

public class StrategySelector : MonoBehaviour
{
    private Dictionary<OpponentStrategy, Action> strategiesMap;
    private AgentBehaviour agentBehaviour;
    private AgentMoveBehavior agentMoveBehavior;
    private KeyCollectorBehavior keyCollectorBehavior;
    private OpponentNPCBrain opponentNPCBrain;
    private OpponentBT opponentBT;
    private MLOpponentAgent opponentML;
    private SimpleCharacterController opponentMLController;
    private Rigidbody rigidBody;

    void Start()
    {
        agentBehaviour = GetComponent<AgentBehaviour>();
        agentMoveBehavior = GetComponent<AgentMoveBehavior>();
        keyCollectorBehavior = GetComponent<KeyCollectorBehavior>();
        opponentNPCBrain = GetComponent<OpponentNPCBrain>();
        opponentBT = GetComponent<OpponentBT>();
        opponentML = GetComponent<MLOpponentAgent>();
        opponentMLController = GetComponent<SimpleCharacterController>();
        rigidBody = GetComponent<Rigidbody>();
        
        strategiesMap = new Dictionary<OpponentStrategy, Action>
        {
            { OpponentStrategy.GOAP, useGoap },
            { OpponentStrategy.BT, useBT },
            { OpponentStrategy.ML, useML }
        };

        GameManager.instance.OnStrategyChange += UpdateStrategy;
        UpdateStrategy(GameManager.instance.Strategy);
    }

    private void useGoap()
    {
        agentBehaviour.enabled = true;
        agentMoveBehavior.enabled = true;
        keyCollectorBehavior.enabled = true;
        opponentNPCBrain.enabled = true;

        opponentBT.enabled = false;
    }

    private void useBT()
    {
        opponentNPCBrain.enabled = false;
        agentBehaviour.enabled = false;
        keyCollectorBehavior.enabled = false;
        agentMoveBehavior.enabled = false;

        opponentBT.enabled = true;
    }

    private void useML()
    {
        opponentNPCBrain.enabled = false;
        agentBehaviour.enabled = false;
        keyCollectorBehavior.enabled = false;
        agentMoveBehavior.enabled = false;
        opponentBT.enabled = false;
    }

    private void UpdateStrategy(OpponentStrategy strategy)
    {
        strategiesMap.TryGetValue(strategy, out var setStrategy);
        setStrategy?.Invoke();
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnStrategyChange -= UpdateStrategy;
        }
    }
}