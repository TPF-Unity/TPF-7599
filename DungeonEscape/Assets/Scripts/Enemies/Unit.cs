using System;
using StarterAssets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    public NPCStats baseStats;
    public NPCStats stats;
    public UnityEvent<float> onHealthChanged;
    public UnityEvent onDied;
    public static EventHandler OnDestroyed;
    public UnitDifficultyManager unitDifficultyManager;

    private Animator _animator;

    private float timeSinceDamaged = 0f;
    private float timeSinceHealed = 0f;

    private int _animAttackSpeed = Animator.StringToHash("AttackSpeed");
    public void Awake()
    {
        stats = Instantiate(baseStats);
        _animator = GetComponent<Animator>();
        if (unitDifficultyManager == null)
        {
            unitDifficultyManager = GetComponent<UnitDifficultyManager>();
        }
    }

    public void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            timeSinceDamaged += Time.deltaTime;
            timeSinceHealed += Time.deltaTime;
            RegenHealth();
        }
        UpdateAttackSpeed();
    }

    private void RegenHealth()
    {
        if (timeSinceDamaged > 7f && timeSinceHealed > 0.1f && stats.Health != stats.MaxHealth)
        {
            stats.Health = Math.Min(stats.MaxHealth, stats.Health + stats.MaxHealth / 100);
            onHealthChanged?.Invoke(stats.Health / stats.MaxHealth * 100);
            timeSinceHealed = 0f;
        }
    }

    private void UpdateAttackSpeed()
    {
        _animator.SetFloat(_animAttackSpeed, stats.AttackSpeed);
    }

    public void Start()
    {
        stats.MaxHealth *= unitDifficultyManager.GetHealthMultiplier();
        stats.Health = stats.MaxHealth;
        stats.MovementSpeed *= unitDifficultyManager.GetSpeedMultiplier();
        stats.Damage *= unitDifficultyManager.GetDamageMultiplier();
        stats.AttackSpeed *= unitDifficultyManager.GetAttackSpeedMultiplier();
        stats.SightRange *= unitDifficultyManager.GetSightRangeMultiplier();
        stats.XPDropped = (int)(stats.XPDropped * unitDifficultyManager.GetXpDropMultiplier());
    }

    public void TakeDamage(float damage)
    {
        TakeDamageFrom(damage, null);
    }

    public void TakeDamageFrom(float damage, Player player)
    {
        stats.TakeDamage(damage);
        timeSinceDamaged = 0f;
        onHealthChanged?.Invoke(stats.Health / stats.MaxHealth * 100);
        if (stats.Health <= 0)
        {
            Transform detailText = GameObject.Find("Canvas").transform.Find("GameOverPanel").Find("Container").Find("DetailText");
            TextMeshProUGUI tmpText = detailText.GetComponent<TextMeshProUGUI>();
            tmpText.text = "You were killed";

            onDied?.Invoke();
            OnDestroyed?.Invoke(this, EventArgs.Empty);
            if (player)
            {
                player.GainXP(stats.XPDropped);
            }

            Destroy(gameObject);
        }
    }
}