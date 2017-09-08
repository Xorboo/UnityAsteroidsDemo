using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerSound))]
[RequireComponent(typeof(PlayerBody))]
public class Player : MonoBehaviour
{
    #region Component getters
    public PlayerMovement Movement
    {
        get
        {
            if (!_Movement)
                _Movement = GetComponent<PlayerMovement>();
            return _Movement;
        }
    }
    PlayerMovement _Movement;

    public PlayerSound Sound
    {
        get
        {
            if (!_Sound)
                _Sound = GetComponent<PlayerSound>();
            return _Sound;
        }
    }
    PlayerSound _Sound;

    public PlayerBody Body
    {
        get
        {
            if (!_Body)
                _Body = GetComponent<PlayerBody>();
            return _Body;
        }
    }
    PlayerBody _Body;
    #endregion


    #region Behaviours
    void OnEnable()
    {
        Body.OnPlayerDied += PlayerDied;
    }

    void OnDisable()
    {
        Body.OnPlayerDied -= PlayerDied;
    }
    #endregion


    public void ResetPlayer()
    {
        Movement.ResetPlayer();
    }

    public void StartMatch()
    {
        Sound.Respawn();
    }


    void PlayerDied()
    {
        Sound.Die();

        MatchManager.Instance.PlayerDied();
    }
}
