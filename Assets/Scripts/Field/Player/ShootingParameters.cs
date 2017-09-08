using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


[CreateAssetMenu(fileName = "ShootingParameters", menuName = "Asteroids/Shooting Parameters")]
public class ShootingParameters : ScriptableObject
{
    [Range(0f, 5f)]
    public float ReloadTime = 0.3f;

    [SerializeField]
    Bullet BulletPrefab = null;
    [SerializeField, Range(0f, 50f)]
    float BulletSpeed = 12f;
    [SerializeField, Range(0f, 10f)]
    float BulletLifeTime = 3f;

    [Space(10)]
    [SerializeField]
    AudioClip ShootClip = null;


    public Bullet SpawnBullet(Transform spawnPoint, Transform root)
    {
        var bullet = BulletPrefab.Spawn(spawnPoint.position, spawnPoint.rotation, root);
        bullet.Init(BulletSpeed, BulletLifeTime);

        SoundKit.Instance.PlayOneShot(ShootClip);
        return bullet;
    }
}
