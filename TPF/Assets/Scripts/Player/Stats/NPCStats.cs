using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCStats", menuName = "Stats/NPCStats")]
public class NPCStats : ScriptableObject
{
    [SerializeField] private float health;
    [SerializeField] private float damage;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float attackSpeed;

    public float Health { get => health; set => health = value; }
    public float Damage { get => damage; set => damage = value; }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public float SprintSpeed { get => sprintSpeed; set => sprintSpeed = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
