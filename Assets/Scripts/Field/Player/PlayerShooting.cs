using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


public class PlayerShooting : MonoBehaviour
{
    public Transform BulletSpawnPoint = null;
    public Transform BulletsRoot = null;

    [Space(10)]
    [SerializeField]
    ShootingParameters Parameters = null;


    IPlayerInput PlayerInput;
    float ReloadLeft = 0f;


    #region Behaviours
    void Update()
    {
        ReloadLeft -= TimeManager.DeltaTime;

        if (PlayerInput.IsShooting())
        {
            if (ReloadLeft <= 0f)
            {
                Parameters.SpawnBullet(BulletSpawnPoint, BulletsRoot);
                ReloadLeft = Parameters.ReloadTime;
            }
        }
    }
    #endregion

    
    public void Clear()
    {
        BulletsRoot.RecycleAllChildren();
    }

    public void SetInput(IPlayerInput playerInput)
    {
        PlayerInput = playerInput;
    }
}
