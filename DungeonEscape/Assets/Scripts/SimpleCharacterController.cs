using System;
using Misc;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SimpleCharacterController : MonoBehaviour
{
    // Movement
    [Tooltip("Maximum slope the character can jump on")] [Range(5f, 60f)]
    public float slopeLimit = 45f;

    [Tooltip("Move speed in meters/second")]
    public float moveSpeed = 5f;

    [Tooltip("Turn speed in degrees/second, left (+) or right (-)")]
    public float turnSpeed = 3000;

    public int ForwardInput { get; set; }
    public int TurnInput { get; set; }

    // Combat
    public int FireInput { get; set; }
    public float FireAngle { get; set; }
    public bool alreadyAttacked = false;
    public GameObject bulletPrefab;
    private Unit unit;
    public float attackSpeedFactor = 1f;


    // Other
    private new Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        unit = GetComponent<Unit>();
    }

    private void CheckGrounded()
    {
        IsGrounded = false;
        var capsuleHeight = Mathf.Max(capsuleCollider.radius * 2f, capsuleCollider.height);
        var capsuleBottom = transform.TransformPoint(capsuleCollider.center - Vector3.up * capsuleHeight / 2f);
        var radius = transform.TransformVector(capsuleCollider.radius, 0f, 0f).magnitude;
        var ray = new Ray(capsuleBottom + transform.up * .01f, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, radius * 5f))
        {
            var normalAngle = Vector3.Angle(hit.normal, transform.up);
            if (normalAngle < slopeLimit)
            {
                var maxDist = radius / Mathf.Cos(Mathf.Deg2Rad * normalAngle) - radius + .02f;
                if (hit.distance < maxDist)
                    IsGrounded = true;
            }
        }
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        ProcessActions();
    }

    private void ExecuteAttack()
    {
        alreadyAttacked = true;
        var normalizedFireAngle = FireAngle % 360;
        if (normalizedFireAngle < 0)
        {
            normalizedFireAngle += 360;
        }

        var rotation = Quaternion.Euler(0, normalizedFireAngle, 0);
        var fireDirection = rotation * Vector3.forward;
        var attackSpawnPosition = transform.position + fireDirection * 1.0f;

        var bullet = Instantiate(bulletPrefab, attackSpawnPosition, rotation);
        bullet.layer = LayerMask.NameToLayer(Layer.PlayerProjectiles.ToString());
        bullet.GetComponent<Bullet>().Damage = unit.stats.Damage;
        var bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.ShootAtDirection(fireDirection);
    }


    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void ProcessActions()
    {
        if (TurnInput != 0f)
        {
            var angle = Mathf.Clamp(TurnInput, -1f, 1f) * turnSpeed;
            transform.Rotate(Vector3.up, Time.fixedDeltaTime * angle);
        }

        if (FireInput != 0 && !alreadyAttacked)
        {
            ExecuteAttack();
            Invoke(nameof(ResetAttack), attackSpeedFactor / unit.stats.AttackSpeed);
        }

        if (IsGrounded)
        {
            var horizontalVelocity = Vector3.ProjectOnPlane(rigidbody.velocity, Vector3.up);
            rigidbody.velocity = horizontalVelocity;

            rigidbody.velocity += transform.forward * (Mathf.Clamp(ForwardInput, -1f, 1f) * moveSpeed);
        }
        else
        {
            if (Mathf.Approximately(ForwardInput, 0f))
            {
                return;
            }

            var verticalVelocity = Vector3.Project(rigidbody.velocity, Vector3.up);
            rigidbody.velocity = verticalVelocity +
                                 transform.forward * (Mathf.Clamp(ForwardInput, -1f, 1f) * moveSpeed) / 2f;
        }
    }
}