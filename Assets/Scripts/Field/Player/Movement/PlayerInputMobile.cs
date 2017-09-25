using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Rand = UnityEngine.Random;


public class PlayerInputMobile : PlayerInputPC
{
    CanvasUIBase UIController = null;


    public override void Init(CanvasUIBase uiController)
    {
        base.Init(uiController);

        UIController = uiController;
    }


    public override MovementData GetMoveInput()
    {
        Assert.IsNotNull(UIController, "Mobile UI was not instantiated");

        var data = UIController.GetMovementData();

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
        Assert.IsNotNull(UIController, "Mobile UI was not instantiated");

        bool isFiring = UIController.IsShooting();
        return isFiring;
    }
}
