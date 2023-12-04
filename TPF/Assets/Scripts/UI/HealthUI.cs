using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{

    public VulnerableUnit targetHealth;
    public Slider slider;

    void OnEnable()
    {
        if (targetHealth)
        {
            targetHealth.onHealthChanged.AddListener(UpdateHealthDisplay);
            targetHealth.onDied.AddListener(HandleDeath);
        }
    }

    void OnDisable()
    {
        if (targetHealth)
        {
            targetHealth.onHealthChanged.AddListener(UpdateHealthDisplay);
            targetHealth.onDied.AddListener(HandleDeath);
        }

    }

    void UpdateHealthDisplay(int newHealth)
    {
        slider.value = newHealth;
    }

    void HandleDeath()
    {
        slider.value = targetHealth.maxHealth;
    }
}