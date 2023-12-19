using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsUIManager : MonoBehaviour
{
    private Unit player;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;
    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>();
    }

    void Update()
    {
        healthText.text = player.stats.Health.ToString();
        attackText.text = player.stats.Damage.ToString();
        attackSpeedText.text = player.stats.AttackSpeed.ToString();
        moveSpeedText.text = player.stats.MovementSpeed.ToString();
    }
}
