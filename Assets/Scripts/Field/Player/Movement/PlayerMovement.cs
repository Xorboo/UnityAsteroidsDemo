using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Rand = UnityEngine.Random;


[RequireComponent(typeof(PlayerShooting))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    MovementController Movement = null;

    PlayerShooting PlayerShooting;


    #region Behaviours
    void Awake()
    {
        Assert.IsNotNull(Movement, "Movement parameters are not set");
        Movement.Init(this);

        PlayerShooting = GetComponent<PlayerShooting>();
        PlayerShooting.SetInput(Movement.PlayerInput);
    }

    void Update()
    {
        Movement.MoveShip(this);
    }
    #endregion


    public void ResetPlayer()
    {
        Movement.Clear(this);
        PlayerShooting.Clear();
    }
}
