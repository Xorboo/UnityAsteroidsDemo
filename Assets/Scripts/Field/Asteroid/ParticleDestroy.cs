using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


[RequireComponent(typeof(ParticleSystem))]
public class ParticleDestroy : MonoBehaviour
{
    ParticleSystem Particles;


    public void Awake()
    {
        Particles = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        if (Particles && !Particles.IsAlive())
        {
            gameObject.Recycle();
        }
    }
}