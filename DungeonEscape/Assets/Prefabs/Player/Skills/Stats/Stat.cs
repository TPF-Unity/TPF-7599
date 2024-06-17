using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

[CreateAssetMenu(menuName = "Unit Stats")]
public class Stats: ScriptableObject
{
    public Dictionary<Stat, float> stats = new Dictionary<Stat, float>();
}
