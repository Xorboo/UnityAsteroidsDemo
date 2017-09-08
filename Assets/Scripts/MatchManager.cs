﻿using System;
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
    Player Player = null;
    [SerializeField]
    GameOverPopup GameOverPopup = null;

    [Space(10)]
    [SerializeField, Range(0f, 10f), Tooltip("Pause after player death")]
    float RestartPause = 2f;

    public bool IsPlaying { get; private set; }

    #region Behaviours
    new void Awake()
    {
        base.Awake();

        IsPlaying = false;
    }
    #endregion


    public void StartMatch()
    {
        StopAllCoroutines();

        StartRound();

        ScoreManager.Instance.StartMatch();
        PlayerHealth.Value = InitialHealth;
    }

    public void StopMatch()
    {
        IsPlaying = false;
    }

    public void PlayerDied()
    {
        PlayerHealth.Value--;

        if (PlayerHealth.Value > 0)
        {
            StartRound(true);
        }
        else
        {
            // Resetting player to remove bullets
            Player.ResetPlayer();
            Player.gameObject.SetActive(false);
            // Showing popup while asteroids flying in the background
            MenuManager.Instance.ShowPopup(GameOverPopup);
        }
    }


    void StartRound(bool hidePlayer = false)
    {
        StartCoroutine(RoundRoutine(hidePlayer));
    }

    IEnumerator RoundRoutine(bool hidePlayer)
    {
        // Clear field from previous round
        IsPlaying = false;
        Player.ResetPlayer();
        Player.gameObject.SetActive(!hidePlayer);
        if (!hidePlayer)
            Player.StartMatch();
        AsteroidSpawner.Instance.Clear();

        yield return new WaitForSeconds(RestartPause);

        // Start new wave
        IsPlaying = true;
        if (hidePlayer)
        {
            Player.gameObject.SetActive(true);
            Player.StartMatch();
        }
        AsteroidSpawner.Instance.StartSpawn();
    }
}
