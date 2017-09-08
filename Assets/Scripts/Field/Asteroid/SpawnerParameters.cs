using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


[CreateAssetMenu(fileName = "SpawnerParameters", menuName = "Asteroids/Spawner Parameters")]
public class SpawnerParameters : ScriptableObject
{
    [SerializeField]
    AsteroidParameters AsteroidParameters = null;
    [SerializeField]
    Asteroid Prefab = null;

    [Space(10)]
    [SerializeField, Range(1, 10), Tooltip("Amount of asteroids in initial wave")]
    int StartingWaveSize = 4;
    [SerializeField, Range(0, 10), Tooltip("Increase of asteroids for each wave")]
    int WaveSizeIncrease = 2;
    [Range(0f, 5f), Tooltip("Pause before wave spawn")]
    public float WaveSpawnPause = 2f;


    Camera MainCamera;
    public int CurrentWave { get; private set; }

    #region Behaviours
    void OnEnable()
    {
        Reset();
    }
    #endregion


    public void Reset()
    {
        CurrentWave = 0;
        MainCamera = Camera.main;
    }

    public int SpawnWave(Transform root)
    {
        Debug.LogFormat("Spawning wave #{0}", CurrentWave);

        int amount = StartingWaveSize + WaveSizeIncrease * CurrentWave;

        for(int i = 0; i < amount; i++)
        {
            Vector3 position = GetSpawnPosition();
            SpawnAsteroid(root, AsteroidParameters, position);
        }

        CurrentWave++;
        return amount;
    }

    public Asteroid SpawnAsteroid(Transform root, AsteroidParameters parameters, Vector3 position)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, Rand.Range(0f, 360f));
        var asteroid = Prefab.Spawn(position, rotation, root);
        asteroid.Init(parameters);
        return asteroid;
    }

    Vector3 GetSpawnPosition()
    {
        int side = Rand.Range(0, 4);

        // Moving far away (half a screen) from screen border on spawn
        // This will cause asteroid to warp 1/2 times in the beginning, but we can avoid a bit of code this way
        float a = 0.5f + (side % 2 == 0 ? -1 : 1);
        float b = Rand.value;

        bool swap = side > 2;
        Vector3 viewport = new Vector3(swap ? a : b, swap ? b : a, 0f);
        Vector3 position = MainCamera.ViewportToWorldPoint(viewport);
        position.z = 0f;
        return position;
    }
}
