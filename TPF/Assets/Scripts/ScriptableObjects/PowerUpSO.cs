using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType {
    Health,
    MovSpeed,
    AttSpeed,
    Damage
}

[CreateAssetMenu()]
public class PowerUpSO : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public PowerUpType type;
}
