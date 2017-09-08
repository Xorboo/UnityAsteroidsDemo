using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


public class MatchManager : Singleton<MatchManager>
{
    public Beep<int> PlayerHealth = new Beep<int>(0);

    [Range(1, 10), Tooltip("Initial player health")]
    public int InitialHealth = 3;
    [SerializeField]
    PlayerMovement Player = null;


    #region Behaviours
    #endregion


    public void StartMatch()
    {
        ResetField();

        ScoreManager.Instance.StartMatch();
        PlayerHealth.Value = InitialHealth;
    }

    public void StopMatch()
    {
        ResetField();
    }

    public void PlayerDied()
    {
        PlayerHealth.Value--;

        if (PlayerHealth.Value > 0)
        {
            ResetField();
        }
        else
        {
            GameManager.Instance.SetState(GameManager.GameState.Menu);
        }
    }

    void ResetField()
    {
        AsteroidSpawner.Instance.StartSpawn();
        Player.ResetPlayer();
    }
}
