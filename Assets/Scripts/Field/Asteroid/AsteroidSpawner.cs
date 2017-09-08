using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


public class AsteroidSpawner : Singleton<AsteroidSpawner>
{
    public Action<Asteroid> OnAsteroidDestroyed = delegate { };


    [SerializeField]
    SpawnerParameters Parameters = null;
    [SerializeField]
    Transform AsteroidsRoot = null;

    // We can change this to store asteroids objects if needed
    int AsteroidsAmount;


    #region Behaviours
    #endregion


    public void StartSpawn()
    {
        Parameters.Reset();

        AsteroidsAmount = 0;
        AsteroidsRoot.RemoveAllChildren();

        SpawnWave();
    }

    public void SpawnAsteroid(AsteroidParameters parameters, Vector3 position)
    {
        Parameters.SpawnAsteroid(AsteroidsRoot, parameters, position);
    }

    void SpawnWave()
    {
        AsteroidsAmount += Parameters.SpawnWave(AsteroidsRoot);
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        AsteroidsAmount--;

        var parameters = asteroid.Parameters;
        OnAsteroidDestroyed(asteroid);

        AsteroidsAmount += parameters.SpawnChildren(asteroid);

        if (AsteroidsAmount == 0)
            SpawnWave();
    }
}
