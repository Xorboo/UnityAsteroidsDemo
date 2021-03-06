﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Rand = UnityEngine.Random;


public class AsteroidSpawner : Singleton<AsteroidSpawner>
{
    public Action<int> OnNewWaveSpawned = delegate { };
    public Action<Asteroid> OnAsteroidDestroyed = delegate { };


    [SerializeField]
    SpawnerParameters Parameters = null;
    [SerializeField]
    Transform AsteroidsRoot = null;
    public Transform ParticlesRoot = null;

    // We can change this to store asteroids objects if needed
    int AsteroidsAmount;


    #region Behaviours
    #endregion


    public void StartSpawn()
    {
        Clear();
        SpawnWave();
    }

    public void Clear()
    {
        Parameters.Reset();

        AsteroidsAmount = 0;
        AsteroidsRoot.RecycleAllChildren();
        ParticlesRoot.RecycleAllChildren();
        StopAllCoroutines();
    }

    public void SpawnAsteroid(AsteroidParameters parameters, Vector3 position)
    {
        Parameters.SpawnAsteroid(AsteroidsRoot, parameters, position);
    }

    void SpawnWave()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        // Ignore pause on the first way, as we have additional pause in the beginning
        if (Parameters.CurrentWave > 0)
            yield return new WaitForSeconds(Parameters.WaveSpawnPause);
        AsteroidsAmount += Parameters.SpawnWave(AsteroidsRoot);

        OnNewWaveSpawned(Parameters.CurrentWave);
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        AsteroidsAmount--;

        var parameters = asteroid.Parameters;
        OnAsteroidDestroyed(asteroid);

        AsteroidsAmount += parameters.SpawnChildren(asteroid);

        if (AsteroidsAmount == 0 || AsteroidsRoot.childCount == 0)
        {
            Assert.IsTrue(AsteroidsAmount == 0, "No asteroid objects found, but counter is > 0");
            SpawnWave();
        }
    }
}
