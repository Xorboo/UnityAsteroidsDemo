using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


public class MobileUI : CanvasUIBase
{
    bool IsLeft, IsRight, IsUp, IsFire;


    #region Behaviours
    void OnEnable()
    {
        IsLeft = IsRight = IsUp = IsFire = false;
    }
    #endregion


    public override MovementData GetMovementData()
    {
        return new MovementData
        {
            Acceleration = IsUp ? 1 : 0,
            Rotation = IsRight ? 1 : (IsLeft ? -1 : 0)
        };
    }

    public override bool IsShooting()
    {
        return IsFire;
    }


    #region Callbacks
    public void LeftPressed(bool pressed)
    {
        IsLeft = pressed;
    }

    public void RightPressed(bool pressed)
    {
        IsRight = pressed;
    }

    public void UpPressed(bool pressed)
    {
        IsUp = pressed;
    }

    public void FirePressed(bool pressed)
    {
        IsFire = pressed;
    }
    #endregion
}
