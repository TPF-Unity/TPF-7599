using System;
using System.Collections.Generic;
using System.Linq;
using AI.GOAP.Scripts;
using Misc;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace AI.ML
{
    public class MLOpponentAgent : Agent
    {
        // References
        [SerializeField] public GameManager gameManager;
        public NPCAnimationController AnimationController { get; set; }
        public Agent mlAgent;
        public SimpleCharacterController characterController;
        private Unit _unit;
        private SceneInitializer _sceneInitializer;
        public bool isTraining = true;
        private KeyProgressionManager _keyProgressionManager;
        private GameObject[] _patrolPoints;
        private GameObject[] _keys;


        // Game system
        [SerializeField] public MLTrainingScene trainingScene;
        public GameObject[] startPositions;
        private Vector3[] _startPositionsVector;
        private Vector3 _previousPosition;
        private bool _alreadyShot;
        private RayPerceptionSensorComponent3D _sensor;

        // Reward system
        private const float ConstantRewardDrain = -0.0001f;
        private const float ReachedPatrolPointReward = 2.0f;
        private const float CollectedKeyReward = 4.0f;
        private const float BeingHitRewardDrain = -0.01f;
        private const float BeingKilledRewardDrain = -2f;
        private const float TurningRewardDrain = -0.0005f;
        private const float WalkingStraightReward = 0.0002f;
        private const float ShootingEnemyReward = 0.002f;
        private const float ShootingThinAirRewardDrain = -0.0001f;
        private const float CollectedAllKeysReward = 10f;

        // Position
        private Vector3 _startPosition;
        private Rigidbody _rigidBody;
        private Vector3 latestNearestSpawnPoint;

        // Combat
        private float _angleToClosestEnemy = 0;

        private void logReward()
        {
            var currentReward = mlAgent.GetCumulativeReward();
            Debug.Log($"Current Reward: {currentReward}");
        }

        public void Update()
        {
            if (_keys != null || gameManager.keys == null)
            {
                return;
            }

            _keys = gameManager.keys.Clone() as GameObject[];
            foreach (var key in _keys)
            {
                var keyController = key.GetComponentInChildren<KeyController>();
                keyController.OnKeyCollected += OnKeyCollected;
            }
            
            _sceneInitializer.Restart(trainingScene);
        }

        private void Restart()
        {
            EndEpisode();
        }

        private void OnHealthChanged(float currentHealthPercentage)
        {
            AddReward(BeingHitRewardDrain);
            if (!(currentHealthPercentage <= 0))
            {
                return;
            }

            AddReward(BeingKilledRewardDrain);
            Restart();
        }

        public override void Initialize()
        {
            _keyProgressionManager = GetComponent<KeyProgressionManager>();
            _sensor = GetComponent<RayPerceptionSensorComponent3D>();
            _unit = GetComponent<Unit>();
            _unit.onHealthChanged.AddListener(OnHealthChanged);
            characterController = GetComponent<SimpleCharacterController>();
            _rigidBody = GetComponent<Rigidbody>();
            mlAgent = GetComponent<Agent>();
            AnimationController = GetComponent<NPCAnimationController>();

            _patrolPoints = GameObject.FindGameObjectsWithTag(Tags.PatrolPoint.ToString());

            if (startPositions == null || startPositions.Length == 0)
            {
                startPositions = new[] { gameObject };
            }

            _startPositionsVector = new Vector3[startPositions.Length];

            for (var i = 0; i < startPositions.Length; i++)
            {
                if (startPositions[i] != null)
                    _startPositionsVector[i] = startPositions[i].transform.position;
            }

            if (isTraining)
            {
                _sceneInitializer = new SceneInitializer();
                _sceneInitializer.Initialize(gameManager, _startPositionsVector, gameObject, trainingScene);
            }
            gameManager.Initialize();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            var nearestSpawnPoint = FindNearestSpawnPoint(transform.position, sensor) ??
                                    _patrolPoints[0];

            var waypointPositionInLocalCoords =
                transform.InverseTransformPoint(nearestSpawnPoint.transform.position);
            sensor.AddObservation(waypointPositionInLocalCoords);
            sensor.AddObservation(waypointPositionInLocalCoords.magnitude);
            sensor.AddObservation(_rigidBody.velocity);
            sensor.AddObservation(transform.forward);
            sensor.AddObservation(characterController.alreadyAttacked);
            sensor.AddObservation(_angleToClosestEnemy != 0);
        }

        private GameObject FindNearestSpawnPoint(Vector3 currentPosition, VectorSensor sensor)
        {
            if (_patrolPoints == null || _patrolPoints.Length == 0)
            {
                Debug.LogError("Spawn positions array is empty or not set!");
                return null;
            }

            var spawnPositions = _patrolPoints;
            if (_keyProgressionManager.HasAllKeys())
            {
                spawnPositions = gameManager.doorSpawnPositions;
            }

            GameObject nearestGameObject = null;
            var nearestDistance = float.MaxValue;

            foreach (var spawnPoint in spawnPositions)
            {
                var distance = Vector3.Distance(currentPosition, spawnPoint.transform.position);
                if (!spawnPoint.activeInHierarchy || !(distance < nearestDistance))
                {
                    continue;
                }

                nearestDistance = distance;
                nearestGameObject = spawnPoint;
            }

            if (nearestGameObject != null)
            {
                latestNearestSpawnPoint = nearestGameObject.transform.position;
            }

            return nearestGameObject;
        }

        public override void OnEpisodeBegin()
        {
            Debug.Log("Episode Beggining");
            transform.position = _startPositionsVector[0];
            transform.rotation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f));
            _rigidBody.velocity = Vector3.zero;
            if (isTraining)
            {
                _keyProgressionManager.Reset();
                _unit.stats.Health = _unit.stats.MaxHealth;
                _sensor.RayLayerMask &= ~(1 << LayerMask.NameToLayer(Layer.Doors.ToString()));
                _sceneInitializer.Restart(trainingScene);
                //gameManager.Initialize();
            }
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
            var horizontal = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
            var shoot = Input.GetMouseButton(0) ? 1 : 0;

            var actions = actionsOut.DiscreteActions;
            actions[0] = vertical >= 0 ? vertical : 2;
            actions[1] = horizontal >= 0 ? horizontal : 2;
            actions[2] = shoot;
        }

        private void OnKeyCollected(GameObject key)
        {
            AddReward(CollectedKeyReward);
            _keys = _keys.Where(obj => obj != key).ToArray();
            if (_keyProgressionManager.HasAllKeys())
            {
                Debug.Log("Setting Ray Layer Mask to show Doors");
                _sensor.RayLayerMask |= 1 << LayerMask.NameToLayer(Layer.Doors.ToString());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.PatrolPoint.ToString()) && _patrolPoints.Contains(other.gameObject))
            {
                AddReward(ReachedPatrolPointReward);
                _patrolPoints = _patrolPoints.Where(obj => obj != other.gameObject).ToArray();
            }

            // if (other.CompareTag(Tags.KeySpawn.ToString()) && _keys.Contains(other.gameObject))
            // {
            //     AddReward(CollectedKeyReward);
            //     _keys = _keys.Where(obj => obj != other.gameObject).ToArray();
            //     if (_keyProgressionManager.HasAllKeys())
            //     {
            //         Debug.Log("Setting Ray Layer Mask to show Doors");
            //         _sensor.RayLayerMask |= 1 << LayerMask.NameToLayer(Layer.Doors.ToString());
            //     }
            // }

            if (!_keyProgressionManager.HasAllKeys() || !other.CompareTag(Tags.Door.ToString()))
            {
                return;
            }

            AddReward(CollectedAllKeysReward);
            Restart();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 15f);
            Gizmos.DrawLine(transform.position, latestNearestSpawnPoint);
        }

        private float GetAngleToClosestEnemy()
        {
            var enemies = Physics.OverlapSphere(transform.position, 15f, 1 << 8 | 1 << 6);
            enemies = enemies.Where(enemyCollider => enemyCollider.transform.position != transform.position).ToArray();


            if (enemies.Length <= 0)
            {
                return 0;
            }

            var closestDistance = Vector3.Distance(transform.position, enemies[0].transform.position);
            var closestPosition = enemies[0].transform.position;
            foreach (var enemy in enemies)
            {
                if (enemy.gameObject == gameObject)
                {
                    continue;
                }

                var distanceToCurrent = Vector3.Distance(transform.position, enemy.transform.position);
                if (!(distanceToCurrent < closestDistance))
                {
                    continue;
                }

                closestDistance = distanceToCurrent;
                closestPosition = enemy.transform.position;
            }

            var directionToEnemy = Vector3.Normalize(closestPosition - transform.position);

            var angle = Vector3.Angle(Vector3.forward, directionToEnemy);
            var crossProduct = Vector3.Cross(Vector3.forward, directionToEnemy);

            if (crossProduct.y < 0)
            {
                angle = -angle;
            }

            return angle;
        }

        private void PenalizeTurning(int horizontal)
        {
            if (horizontal is 1 or -1)
            {
                AddReward(TurningRewardDrain);
            }
        }

        private bool movedForward()
        {
            Vector3 movementDirection = transform.position - _previousPosition;
            return Vector3.Dot(movementDirection, transform.forward) > 0;
        }

        private void RewardWalkingStraight(int vertical)
        {
            if (vertical > 0 && movedForward())
            {
                AddReward(WalkingStraightReward);
            }
        }

        private void RewardOrPenalizeShooting(int shoot)
        {
            if (shoot == 0)
            {
                return;
            }

            if (!characterController.alreadyAttacked && _angleToClosestEnemy != 0)
            {
                AddReward(ShootingEnemyReward);
            }
            else
            {
                AddReward(ShootingThinAirRewardDrain);
            }
        }


        public override void OnActionReceived(ActionBuffers actions)
        {
            logReward();
            var vertical = actions.DiscreteActions[0] <= 1 ? actions.DiscreteActions[0] : -1;
            var horizontal = actions.DiscreteActions[1] <= 1 ? actions.DiscreteActions[1] : -1;
            var shoot = actions.DiscreteActions[2];

            _angleToClosestEnemy = GetAngleToClosestEnemy();
            characterController.ForwardInput = vertical;
            characterController.TurnInput = horizontal;
            characterController.FireInput = shoot;
            characterController.FireAngle = _angleToClosestEnemy;

            AddReward(ConstantRewardDrain);

            RewardOrPenalizeShooting(shoot);
            PenalizeTurning(horizontal);
            RewardWalkingStraight(vertical);

            if (transform.position.y < -3f)
            {
                Restart();
            }

            _previousPosition = transform.position;
        }
    }
}