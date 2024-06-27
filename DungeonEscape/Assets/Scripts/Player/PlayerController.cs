﻿using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Unit))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private NPCStats stats;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.10f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 20.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")] public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.2f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        public GameObject bulletPrefab;
        public Transform attackSpawnPoint;
        public float spawnPointRadius = 0f;

        private BasicAttack basicAttack;

        public Unit playerUnit;
        private Player _player;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        private Camera _camera;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animAttack;
        private int _animShoot;
        private int _animIsAttacking;
        private int _animAttackSpeed;
        private PlayerInput _playerInput;
        private Animator _animator;
        private CharacterController _controller;
        private PlayerInputs _input;

        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get { return _playerInput.currentControlScheme == "KeyboardMouse"; }
        }


        private void Awake()
        {
            //powerUpList = new List<PowerUp>();
            _player = GetComponentInParent<MainPlayer>();

            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }

            _camera = Camera.main;
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<PlayerInputs>();
            _playerInput = GetComponent<PlayerInput>();
            basicAttack = GetComponent<BasicAttack>();

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            stats = playerUnit.stats;
            playerUnit.onHealthChanged?.Invoke(stats.Health / stats.MaxHealth * 100);
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            JumpAndGravity();
            GroundedCheck();
            StartAttack();
            Move();
            //CheckPowerUps();
            Shoot();
        }

        private void AssignAnimationIDs()
        {
            _animIDJump = Animator.StringToHash("Jump");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animAttack = Animator.StringToHash("Attack");
            _animShoot = Animator.StringToHash("Shoot");
            _animIsAttacking = Animator.StringToHash("isAttacking");
            _animAttackSpeed = Animator.StringToHash("AttackSpeed");
        }

        private Vector3 CalculateTargetPositionFromCursor()
        {
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, attackSpawnPoint.position.y, 0));
            groundPlane.Raycast(ray, out float position);
            return ray.GetPoint(position);
        }

        private void StartAttack()
        {
            if (_input.shoot)
            {
                if (!Grounded) return;
                if (_hasAnimator)
                {
                    if (_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackStart") || _animator.GetCurrentAnimatorStateInfo(0).IsName("AttackEnd")) return;
                    _animator.SetTrigger(_animAttack);
                    _animator.SetBool(_animIsAttacking, true);
                }
            }
        }

        public void Shoot()
        {
            if (_animator.GetBool(_animShoot))
            {
                _animator.SetBool(_animShoot, false);
                basicAttack.Execute(Layer.PlayerProjectiles.ToString(), attackSpawnPoint.position,
                CalculateTargetPositionFromCursor(), stats.AttackSpeed, _player);
                _animator.ResetTrigger("Shoot");
            }

        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? stats.SprintSpeed : stats.MovementSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
            }

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            _speed = targetSpeed;

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);


            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center),
                        FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center),
                    FootstepAudioVolume);
            }
        }
    }
}