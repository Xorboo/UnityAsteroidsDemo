﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


[CreateAssetMenu(fileName = "MovementParameters", menuName = "Asteroids/Movement Parameters")]
public class MovementController : ScriptableObject
{
    [Header("UI for platforms")]
    [SerializeField]
    List<CanvasInputPrefab> CanvasUIControllers = null;
    [SerializeField]
    CanvasUIBase DefaultUI = null;

    [Header("Movement parameters")]
    [SerializeField, Tooltip("Maximum velocity, units/s")]
    float MaximumVelocity = 10f;
    [SerializeField, Range(0, 100f), Tooltip("Acceleration, units/s^2")]
    float Acceleration = 10f;
    [SerializeField, Range(0f, 100f), Tooltip("Deceleration, units/s^2")]
    float Deceleration = 5f;
    [SerializeField, Range(0f, 10f), Tooltip("Deceleration without thrusting, units/s^2")]
    float FrictionDeceleration = 2f;

    [SerializeField, Tooltip("Rotation speed degrees/s")]
    float RotationSpeed = 90f;


    public IPlayerInput PlayerInput { get; private set; }
    Vector3 Velocity;


    public void Init(PlayerMovement ship)
    {
        var uiController = SpawnUI();

        PlayerInput = CreatePlayerInput();
        PlayerInput.Init(uiController);
        Clear(ship);
    }

    public void Clear(PlayerMovement ship)
    {
        Velocity = Vector3.zero;

        var transform = ship.transform;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    public MovementData MoveShip(PlayerMovement ship)
    {
        var movement = PlayerInput.GetMoveInput();
        float dt = TimeManager.DeltaTime;
        var transform = ship.transform;

        if (!Mathf.Approximately(movement.Acceleration, 0f))
        {
            // Accelerating in current forward direction
            float accelerationAmount = movement.Acceleration >= 0 ? Acceleration : Deceleration;
            float velocityChange = movement.Acceleration * accelerationAmount * dt;
            Velocity = Vector3.ClampMagnitude(Velocity + velocityChange * transform.up, MaximumVelocity);
        }
        else
        {
            // Decellerating otherwise
            Vector3 direction = Velocity.normalized;
            float velocity = Velocity.magnitude;

            float lostVelicuty = FrictionDeceleration * dt;
            Velocity = direction * Mathf.MoveTowards(velocity, 0f, lostVelicuty);
        }

        transform.Translate(Velocity * dt, Space.World);
        transform.Rotate(Vector3.forward, -movement.Rotation * RotationSpeed * dt);

        return movement;
    }


    IPlayerInput CreatePlayerInput()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        return new PlayerInputPC();
#elif UNITY_ANDROID || UNITY_IOS
        return new PlayerInputMobile();
#else
        Debug.LogErrorFormat("No input for target platform: {0}, using default", Application.platform);
        return new PlayerInputPC();
#endif
    }


    #region UI
    CanvasUIBase SpawnUI()
    {
        var prefab = GetUIPrefab();

        if (prefab != null)
        {
            var root = GameManager.Instance.RootUI;
            var ui = Instantiate(prefab, root);
            return ui;
        }
        else
            return null;
    }

    CanvasUIBase GetUIPrefab()
    {
        var platform = ApplicationExtensions.Platform;
        foreach(var ui in CanvasUIControllers)
        {
            if (ui.Platform == platform)
                return ui.Prefab;
        }
        return DefaultUI;
    }

    [Serializable]
    class CanvasInputPrefab
    {
        public CanvasUIBase Prefab = null;
        public RuntimePlatform Platform = RuntimePlatform.Android;
    }
    #endregion
}
