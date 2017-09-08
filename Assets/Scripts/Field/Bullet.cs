using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Rand = UnityEngine.Random;
using UnityConstants;


public class Bullet : MonoBehaviour
{
    public bool HasTriggered { get; private set; }
    Vector3 Velocity;
    float TimeLeft;


    #region Behaviours
    void OnEnable()
    {
        HasTriggered = false;
    }

    void Update()
    {
        float dt = TimeManager.DeltaTime;
        transform.Translate(Velocity * dt, Space.World);

        TimeLeft -= dt;
        if (TimeLeft < 0f)
            RemoveBullet();
    }
    #endregion


    public void Init(float velocity, float lifeTime)
    {
        Assert.IsTrue(velocity > 0, "Bullet velocity should be positive");
        Velocity = transform.up * velocity;

        Assert.IsTrue(velocity > 0, "Bullet lifetime should be positive");
        TimeLeft = lifeTime;
    }

    public void Hit()
    {
        if (!HasTriggered)
        {
            HasTriggered = true;
            RemoveBullet();
        }
    }

    void RemoveBullet()
    {
        gameObject.Recycle();
    }
}
