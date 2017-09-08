using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Rand = UnityEngine.Random;
using UnityConstants;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(ScreenWrap))]
public class Asteroid : MonoBehaviour
{
    [SerializeField]
    AudioClip DeathClip = null;
    [SerializeField]
    GameObject ParticlesPrefab = null;


    #region Component getters
    public SpriteRenderer Renderer
    {
        get
        {
            if (!_Renderer)
            {
                _Renderer = GetComponent<SpriteRenderer>();
                Assert.IsNotNull(_Renderer, "No SpriteRenderer on asteroid");
            }
            return _Renderer;
        }
    }
    SpriteRenderer _Renderer;

    public CircleCollider2D Collider
    {
        get
        {
            if (!_Collider)
            {
                _Collider = GetComponent<CircleCollider2D>();
                Assert.IsNotNull(_Collider, "No CircleCollider2D on asteroid");
            }
            return _Collider;
        }
    }
    CircleCollider2D _Collider;

    public ScreenWrap ScreenWrap
    {
        get
        {
            if (!_ScreenWrap)
            {
                _ScreenWrap = GetComponent<ScreenWrap>();
                Assert.IsNotNull(_Collider, "No ScreenWrap on asteroid");
            }
            return _ScreenWrap;
        }
    }
    ScreenWrap _ScreenWrap;
    #endregion


    public int Health
    {
        get { return _Health; }
        set
        {
            _Health = value;
            if (_Health <= 0)
                Die();
        }
    }
    int _Health = 0;

    bool IsDead;


    public Vector3 Velocity { get; set; }
    public float RotationVelocity { get; set; }

    public AsteroidParameters Parameters { get; private set; }
    

    #region Behaviours
    void Update()
    {
        float dt = TimeManager.DeltaTime;
        transform.Translate(Velocity * dt, Space.World);
        transform.Rotate(Vector3.forward, RotationVelocity * dt);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;
        switch (layer)
        {
            case Layers.Bullet:
                var bullet = collision.gameObject.GetComponent<Bullet>();
                Assert.IsNotNull(bullet, "Cant find bullet component in collision");
                if (!bullet.HasTriggered)
                {
                    Health--;
                    bullet.Hit();
                }
                break;

            case Layers.Player:
                // Checked in PlayerBody
                break;

            default:
                Debug.LogErrorFormat("Unsupported collision with asteroid, layer: {0}", layer);
                Die();
                break;
        }
    }
    #endregion


    public void Init(AsteroidParameters parameters)
    {
        Parameters = parameters;
        Parameters.InitAsteroid(this);
        ScreenWrap.SetPadding(Collider.radius);

        IsDead = false;
        Collider.enabled = true;
    }


    public void Die()
    {
        if (!IsDead)
        {
            IsDead = true;

            if (MatchManager.Instance.IsPlaying)
            {
                AsteroidSpawner.Instance.AsteroidDestroyed(this);

                // TODO Play sprite animation there if needed
                Collider.enabled = false;
                SoundKit.Instance.PlayOneShot(DeathClip);
                ParticlesPrefab.Spawn(transform.position, Quaternion.identity, AsteroidSpawner.Instance.ParticlesRoot);
            }
            gameObject.Recycle();
        }
    }
}
