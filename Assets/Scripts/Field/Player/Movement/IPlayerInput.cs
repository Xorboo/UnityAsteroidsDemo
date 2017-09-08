using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


public interface IPlayerInput
{
    void Init();
    MovementData GetMoveInput();
    bool IsShooting();
}


public struct MovementData
{
    // Acceleration keys indicator [-1, 1]
    public int Acceleration;
    // Rotation keys indicator [-1, 1]
    public int Rotation;


    public override string ToString()
    {
        return String.Format("Acc: {0}, Rot: {1}", Acceleration, Rotation);
    }
}