﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Rand = UnityEngine.Random;


public class PlayerInputMobile : PlayerInputPC
{
    MobileUI MobileUI = null;


    public override void Init()
    {
        base.Init();

        if (!MobileUI)
            MobileUI = GameManager.Instance.SpawnMobileUI();
    }


    public override MovementData GetMoveInput()
    {
        Assert.IsNotNull(MobileUI, "Mobile UI was not instantiated");

        var data = MobileUI.GetMovementData();

#if UNITY_EDITOR
        // Enabling keyboard controls while in editor, so we can test the game easier
        var keyboardData = base.GetMoveInput();
        if (!Mathf.Approximately(keyboardData.Acceleration, 0f))
            data.Acceleration = keyboardData.Acceleration;
        if (!Mathf.Approximately(keyboardData.Rotation, 0f))
            data.Rotation = keyboardData.Rotation;
#endif
        return data;
    }

    public override bool IsShooting()
    {
        Assert.IsNotNull(MobileUI, "Mobile UI was not instantiated");

        bool isFiring = MobileUI.IsShooting();
        return isFiring;
    }
}
