using AI.GOAP.Config;
using AI.GOAP.Goals;
using AI.GOAP.Sensors;
using AI.Sensors;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace AI.GOAP.Behaviors
{
    [RequireComponent(typeof(AgentBehaviour))]
    public class OpponentNPCBrain : MonoBehaviour
    {
        [SerializeField] private PlayerSensor PlayerSensor;
        [SerializeField] public KeyCollectorBehavior KeyCollector;

        [SerializeField] private AttackConfigSO AttackConfig;
        public AgentBehaviour AgentBehaviour;
        private bool PlayerIsInRange;

        private void Awake()
        {
            AgentBehaviour = GetComponent<AgentBehaviour>();
        }

        private void OnEnable()
        {
            AgentBehaviour.Events.OnActionStop += OnActionStop;
            AgentBehaviour.Events.OnNoActionFound += OnNoActionFound;
            AgentBehaviour.Events.OnGoalCompleted += OnGoalCompleted;
            PlayerSensor.OnPlayerEnter += PlayerSensorOnPlayerEnter;
            PlayerSensor.OnPlayerExit += PlayerSensorOnPlayerExit;
        }

        private void OnDisable()
        {
            AgentBehaviour.Events.OnActionStop -= OnActionStop;
            AgentBehaviour.Events.OnNoActionFound -= OnNoActionFound;
            AgentBehaviour.Events.OnGoalCompleted -= OnGoalCompleted;
            PlayerSensor.OnPlayerEnter -= PlayerSensorOnPlayerEnter;
            PlayerSensor.OnPlayerExit -= PlayerSensorOnPlayerExit;
        }

        private void Start()
        {
            AgentBehaviour.SetGoal<WanderGoal>(false);

            PlayerSensor.Collider.radius = AttackConfig.SensorRadius;
        }

        private void OnNoActionFound(IGoalBase goal)
        {
            AgentBehaviour.SetGoal<WanderGoal>(false);
        }

        private void OnGoalCompleted(IGoalBase goal)
        {
            AgentBehaviour.SetGoal<WanderGoal>(false);
        }

        private void OnActionStop(IActionBase action)
        {
            if (AgentBehaviour.CurrentGoal is KillPlayerGoal && PlayerIsInRange)
                return;

            this.DetermineGoal();
        }
        
        private void DetermineGoal()
        {
            if (PlayerIsInRange)
            {
                AgentBehaviour.SetGoal<KillPlayerGoal>(false);
                return;
            }
            
            if (KeyCollector.KeysRemaining > 0 && !KeyCollector.KeyNotFound)
            {
                AgentBehaviour.SetGoal<CollectKeysGoal>(false);
                return;
            }
            
            AgentBehaviour.SetGoal<WanderGoal>(true);
            KeyCollector.KeyNotFound = false;
        }
        
        private void PlayerSensorOnPlayerEnter(Transform Player)
        {
            PlayerIsInRange = true;
            AgentBehaviour.SetGoal<KillPlayerGoal>(false);
        }

        private void PlayerSensorOnPlayerExit(Vector3 LastKnownPosition)
        {
            PlayerIsInRange = false;
            if (KeyCollector.KeysRemaining > 0 && AgentBehaviour.CurrentGoal is CollectKeysGoal)
            {
                return;
            }

            AgentBehaviour.SetGoal<WanderGoal>(false);
        }
    }
}