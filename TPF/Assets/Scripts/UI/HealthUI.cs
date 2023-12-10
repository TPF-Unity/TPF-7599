using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{

    public Unit unit;
    public Slider slider;


    void Awake()
    {
        unit = GetComponentInParent<Unit>();
    }

    void OnEnable()
    {
        if (unit)
        {
            unit.onHealthChanged.AddListener(UpdateHealthDisplay);
            unit.onDied.AddListener(HandleDeath);
        }
    }

    void OnDisable()
    {
        if (unit)
        {
            unit.onHealthChanged.AddListener(UpdateHealthDisplay);
            unit.onDied.AddListener(HandleDeath);
        }

    }

    void UpdateHealthDisplay(int newHealth)
    {
        slider.value = newHealth;
    }

    void HandleDeath()
    {
    }
}