using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


public abstract class CanvasUIBase : MonoBehaviour
{
    public abstract MovementData GetMovementData();
    public abstract bool IsShooting();
}
