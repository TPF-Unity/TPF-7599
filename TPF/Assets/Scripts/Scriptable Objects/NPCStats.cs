using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCStats", menuName = "Stats/NPCStats")]
public class NPCStats : ScriptableObject
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float damage;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private float sightRange;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float Health { get => health; set => health = value; }
    public float Damage { get => damage; set => damage = value; }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public float SprintSpeed { get => sprintSpeed; set => sprintSpeed = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float SightRange { get => sightRange; set => sightRange = value; }
    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
