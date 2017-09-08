using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


public class PlayerInputPC : IPlayerInput
{
    public void Init()
    {
    }


    public MovementData GetMoveInput()
    {
        var data = new MovementData()
        {
            Acceleration = Mathf.RoundToInt(Input.GetAxis("Acceleration")),
            Rotation = Mathf.RoundToInt(Input.GetAxis("Rotation"))
        };
        return data;
    }

    public bool IsShooting()
    {
        return Input.GetAxis("Fire") > 0;
    }
}
