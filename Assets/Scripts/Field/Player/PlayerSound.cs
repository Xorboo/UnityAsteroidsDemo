using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


public class PlayerSound : MonoBehaviour
{
    [SerializeField]
    AudioClip SpawnClip = null;
    [SerializeField]
    AudioClip DeathClip = null;

    
    public void Respawn()
    {
        SoundKit.Instance.PlayOneShot(SpawnClip);
    }

    public void Die()
    {
        SoundKit.Instance.PlayOneShot(DeathClip);
    }
}
