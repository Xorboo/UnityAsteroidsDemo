using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


public class PlayerInputPC : IPlayerInput
{
    public virtual void Init(CanvasUIBase uiController)
    {
    }


    public virtual MovementData GetMoveInput()
    {
        var data = new MovementData()
        {
            Acceleration = Mathf.RoundToInt(Input.GetAxis("Acceleration")),
            Rotation = Mathf.RoundToInt(Input.GetAxis("Rotation"))
        };
        return data;
    }

    public virtual bool IsShooting()
    {
        return Input.GetAxis("Fire") > 0;
    }
}
