using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Rand = UnityEngine.Random;


[RequireComponent(typeof(PlayerShooting))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    MovementController Movement = null;

    [Space(10)]
    [SerializeField]
    Thrusters ThrustersParticles = null;


    PlayerShooting PlayerShooting;


    #region Behaviours
    void Awake()
    {
        Assert.IsNotNull(Movement, "Movement parameters are not set");
        Movement.Init(this);

        PlayerShooting = GetComponent<PlayerShooting>();
        PlayerShooting.SetInput(Movement.PlayerInput);
    }

    void Update()
    {
        var movement = Movement.MoveShip(this);
        ThrustersParticles.Update(movement);
    }
    #endregion


    public void ResetPlayer()
    {
        Movement.Clear(this);
        PlayerShooting.Clear();
    }


    [Serializable]
    class Thrusters
    {
        public ParticleSystem Main = null;
        public ParticleSystem Left = null;
        public ParticleSystem Right = null;

        public void Update(MovementData movement)
        {
            SetEmission(Main, movement.Acceleration > 0f);
            SetEmission(Left, movement.Rotation > 0f);
            SetEmission(Right, movement.Rotation < 0f);
        }

        void SetEmission(ParticleSystem ps, bool enabled)
        {
            var em = ps.emission;
            em.enabled = enabled;
        }
    }
}
