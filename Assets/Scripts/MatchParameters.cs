using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


[CreateAssetMenu(fileName = "MatchParameters", menuName = "Asteroids/Match Parameters")]
public class MatchParameters : ScriptableObject
{
    [Range(1, 10), Tooltip("Initial player health")]
    public int InitialHealth = 3;
    [Range(0f, 10f), Tooltip("Pause after player death")]
    public float RestartPause = 2f;
}
