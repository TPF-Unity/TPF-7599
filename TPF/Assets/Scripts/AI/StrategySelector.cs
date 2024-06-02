using System.Collections;
using System.Collections.Generic;
using AI.GOAP.Behaviors;
using CrashKonijn.Goap.Behaviours;
using UnityEngine;

public class StrategySelector : MonoBehaviour
{
    void Start() {
        GameManager.instance.OnStrategyChange += UpdateStrategy;
        UpdateStrategy(GameManager.instance.UseGOAP);
    }

    private void UpdateStrategy(bool goap) {
        AgentBehaviour agentBehaviour = GetComponent<AgentBehaviour>();
        AgentMoveBehavior agentMoveBehavior = GetComponent<AgentMoveBehavior>();
        KeyCollectorBehavior keyCollectorBehavior = GetComponent<KeyCollectorBehavior>();
        OpponentNPCBrain opponentNPCBrain = GetComponent<OpponentNPCBrain>();

        OpponentBT opponentBT = GetComponent<OpponentBT>();

        if (goap) {
            agentBehaviour.enabled = true;
            agentMoveBehavior.enabled = true;
            keyCollectorBehavior.enabled = true;
            opponentNPCBrain.enabled = true;

            opponentBT.enabled = false;
        } else {
            opponentNPCBrain.enabled = false;
            agentBehaviour.enabled = false;
            keyCollectorBehavior.enabled = false;
            agentMoveBehavior.enabled = false;

            opponentBT.enabled = true;
        }
    }

    private void OnDestroy() {
        if (GameManager.instance != null) {
            GameManager.instance.OnStrategyChange -= UpdateStrategy;
        }
    }
}
