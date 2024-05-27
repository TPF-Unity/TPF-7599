using AI.Sensors;
using BehaviourTree;
using UnityEngine;

public class TaskPlayerInRange : Node {
    private readonly PlayerSensor sensor;
    readonly ItemInfo itemInfo;


    public TaskPlayerInRange(ItemInfo itemInfo, PlayerSensor playerSensor) {
        sensor = playerSensor;
        this.itemInfo = itemInfo;
        sensor.SetRadius(itemInfo.detectionRange);

        sensor.OnPlayerEnter += PlayerSensorOnPlayerEnter;
        sensor.OnPlayerExit += PlayerSensorOnPlayerExit;
    }

    public override NodeState Evaluate() {
        object t = GetData(BTContextKey.Player);

        if (t == null) {
            state = NodeState.FAILURE;
            return state;
        } else {
            state = NodeState.SUCCESS;
            return state;
        }
    }

    private void PlayerSensorOnPlayerEnter(Transform player) {
        parent.SetData(BTContextKey.Player, player);
    }
    private void PlayerSensorOnPlayerExit(Vector3 lastPosition) {
        ClearData(BTContextKey.Player);
    }
}