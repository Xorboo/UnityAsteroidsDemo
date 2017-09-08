//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    Vector3 _Velocity;

    public Vector3 Velocity
    {
        get
        {
            return _Velocity;
        }
        set
        {
            if (_Velocity != value)
            {
                _Velocity = value;
                VelocityChanged();
            }
        }
    }

    public float Velocity2D
    {
        get
        {
            return _Velocity.z;
        }
        set
        {
            if (float.Epsilon < Mathf.Abs(_Velocity.z - value))
            {
                _Velocity.z = value;
                VelocityChanged();
            }
        }
    }


    #region Behaviours
    void Start()
    {
        VelocityChanged();
    }

    void Update()
    {
        transform.Rotate(_Velocity * TimeManager.DeltaTime);
    }
    #endregion

    void VelocityChanged()
    {
        enabled = _Velocity != Vector3.zero;
    }
}
