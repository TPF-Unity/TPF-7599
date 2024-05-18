using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float fixedDeltaTime;

    public void Start()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }
}