using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Rand = UnityEngine.Random;
using UnityConstants;


public class Bullet : MonoBehaviour
{
    [SerializeField, Tooltip("Time after spawn, when collisions with player ship are ignored")]
    float IgnorePlayerTime = 0.2f;

    public bool IgnorePlayerCollision
    {
        get { return CurrentTime <= IgnorePlayerTime; }
    }

    public bool HasTriggered { get; private set; }
    Vector3 Velocity;
    float CurrentTime;
    float LifeTime;


    #region Behaviours
    void OnEnable()
    {
        HasTriggered = false;
    }

    void Update()
    {
        float dt = TimeManager.DeltaTime;
        transform.Translate(Velocity * dt, Space.World);

        CurrentTime += dt;
        if (CurrentTime >= LifeTime)
            RemoveBullet();
    }
    #endregion


    public void Init(float velocity, float lifeTime)
    {
        Assert.IsTrue(velocity > 0, "Bullet velocity should be positive");
        Velocity = transform.up * velocity;

        Assert.IsTrue(lifeTime > 0, "Bullet lifetime should be positive");
        CurrentTime = 0;
        LifeTime = lifeTime;
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
