using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Rand = UnityEngine.Random;


[CreateAssetMenu(fileName = "AsteroidParameters", menuName = "Asteroids/Asteroid Parameters")]
public class AsteroidParameters : ScriptableObject
{
    [SerializeField, Tooltip("Velocity, units/s")]
    MinMaxFloat Velocity = new MinMaxFloat(1, 2);
    [SerializeField, Tooltip("Rotation velocity, degrees/s")]
    MinMaxFloat RotationVelocity = new MinMaxFloat(15, 90);
    [SerializeField, Tooltip("Possible sprites")]
    List<Sprite> Sprites = null;
    [SerializeField]
    float Radius = 1f;
    [SerializeField, Range(1, 10), Tooltip("Amount of hits needed to destroy asteroid")]
    int Health = 1;
    [Tooltip("Amount of points awarded for destroying asteroid")]
    public int Score = 10;
    [SerializeField]
    Spawn DestroySpawner = null;


    public void InitAsteroid(Asteroid asteroid)
    {
        asteroid.Renderer.sprite = Sprites.Random();
        asteroid.Collider.radius = Radius;
        asteroid.Health = Health;
        Assert.IsTrue(Score >= 0, "Asteroid score should be >= 0");

        float direction = Rand.Range(0f, 2 * Mathf.PI);
        float velocity = Velocity.Random();
        asteroid.Velocity = new Vector3(Mathf.Cos(direction), Mathf.Sin(direction), 0f) * velocity;
        asteroid.RotationVelocity = RotationVelocity.Random() * (Rand.value > 0.5f ? 1 : -1);
    }


    public int SpawnChildren(Asteroid asteroid)
    {
        return DestroySpawner.SpawnChildren(asteroid);
    }


    [Serializable]
    public class Spawn
    {
        public AsteroidParameters Child = null;
        [Range(0, 10)]
        public int Amount;

        
        public int SpawnChildren(Asteroid asteroid)
        {
            if (Child != null)
            {
                Vector3 pos = asteroid.transform.position;

                for (int i = 0; i < Amount; i++)
                {
                    AsteroidSpawner.Instance.SpawnAsteroid(Child, pos);
                }
                return Amount;
            }
            else
                return 0;
        }
    }
}
