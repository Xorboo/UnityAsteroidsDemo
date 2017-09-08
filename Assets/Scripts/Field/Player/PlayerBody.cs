using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Rand = UnityEngine.Random;
using UnityConstants;


public class PlayerBody : MonoBehaviour
{
    public Action OnPlayerDied = delegate { };


    #region Behaviours
    void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;
        switch (layer)
        {
            case Layers.Bullet:
                var bullet = collision.gameObject.GetComponent<Bullet>();
                Assert.IsNotNull(bullet, "Cant find bullet component in collision");
                if (!bullet.HasTriggered && !bullet.IgnorePlayerCollision)
                    OnPlayerDied();
                break;

            case Layers.Enemy:
                OnPlayerDied();
                break;

            default:
                Debug.LogErrorFormat("Unsupported collision with player, layer: {0}", layer);
                break;
        }
    }
    #endregion
}
