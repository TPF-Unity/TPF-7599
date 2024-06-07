using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private List<PowerUp> powerUpList;
    private Unit _unit;
    
    
    public UnityEvent<float> onXPChanged;
    private int lvl = 1;
    private float xp = 0f;
    [SerializeField] private LevelUpInfoSO levelUpInfo;
    [SerializeField] private NPCStats stats;
    public event EventHandler<EventArgs> OnPowerUpChanged;
    
    private void Start()
    {
        _unit = GetComponentInParent<Unit>();
        powerUpList = new List<PowerUp>();
    }


    public List<PowerUp> GetPowerUpList()
    {
        return powerUpList;
    }

    public bool PickUpPowerUp(PowerUp powerUp)
    {
        var stats = _unit.stats;
        if (stats == null)
        {
            return false;
        }
        PowerUp existingPowerUp = powerUpList.Find(pUp => pUp.powerUpSO.type == powerUp.powerUpSO.type);
        if (existingPowerUp != null)
        {
            existingPowerUp.durationLeft = 0f; // remove power up of same type in the next frame
        }

        powerUpList.Add(powerUp);
        OnPowerUpChanged?.Invoke(this, EventArgs.Empty);

        switch (powerUp.powerUpSO.type)
        {
            case PowerUpType.Health:
                stats.Health = Math.Min(stats.MaxHealth, stats.Health + powerUp.floatVal);
                _unit.onHealthChanged?.Invoke(stats.Health / stats.MaxHealth * 100);
                break;
            case PowerUpType.MovSpeed:
                stats.MovementSpeed *= powerUp.floatVal;
                stats.SprintSpeed *= powerUp.floatVal;
                break;
            case PowerUpType.AttSpeed:
                stats.AttackSpeed *= powerUp.floatVal;
                break;
            case PowerUpType.Damage:
                stats.Damage *= powerUp.floatVal;
                break;
            default:
                break;
        }

        return true;
    }

    private void CheckPowerUps()
    {
        var stats = _unit.stats;
        bool powerUpsChanged = false;

        powerUpList.ForEach(powerUp =>
        {
            powerUp.durationLeft -= Time.deltaTime;

            if (powerUp.durationLeft < 0f)
            {
                powerUpsChanged = true;
                // remove effect
                switch (powerUp.powerUpSO.type)
                {
                    case PowerUpType.Health:
                        break;
                    case PowerUpType.MovSpeed:
                        stats.MovementSpeed /= powerUp.floatVal;
                        stats.SprintSpeed /= powerUp.floatVal;
                        break;
                    case PowerUpType.AttSpeed:
                        stats.AttackSpeed /= powerUp.floatVal;
                        break;
                    case PowerUpType.Damage:
                        stats.Damage /= powerUp.floatVal;
                        break;
                    default:
                        break;
                }
            }
        });

        if (powerUpsChanged)
        {
            powerUpList.RemoveAll(powerUp => powerUp.durationLeft < 0f);
            OnPowerUpChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Update()
    {
        CheckPowerUps();
    }

    public void GainXP(float amt)
    {
        xp += amt;
        if (xp >= levelUpInfo.GetXPNeededForLevel(lvl + 1) && lvl < levelUpInfo.maxLevel)
        {
            LevelUp();
        }
        onXPChanged?.Invoke(xp / levelUpInfo.GetXPNeededForLevel(lvl + 1) * 100);
    }
    
    private void addLevelStats(int level)
    {
        if (level < 2)
        {
            return;
        }

        stats.MaxHealth += levelUpInfo.GetStatIncreaseForLevel(level).maxHealth;
        stats.Damage += levelUpInfo.GetStatIncreaseForLevel(level).damage;
        stats.AttackSpeed += levelUpInfo.GetStatIncreaseForLevel(level).attSpeed;
        stats.MovementSpeed += levelUpInfo.GetStatIncreaseForLevel(level).MovSpeed;
        stats.SprintSpeed += levelUpInfo.GetStatIncreaseForLevel(level).MovSpeed;
    }

    private void LevelUp()
    {
        xp -= levelUpInfo.GetXPNeededForLevel(lvl + 1);
        lvl += 1;

        addLevelStats(lvl);

        stats.Health = stats.MaxHealth;
        _unit.onHealthChanged?.Invoke(stats.Health / stats.MaxHealth * 100);
    }

    public float GetXP()
    {
        return xp;
    }

    public int GetLV()
    {
        return lvl;
    }
    
    public int KeysCollected { get; set; }
}