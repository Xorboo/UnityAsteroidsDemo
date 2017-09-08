//
// Copyright (c) Kirill Korepanov. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Beep<T>
{
    public Action<T> OnChanged = delegate { };

    public T Value
    {
        get
        {
            return _Value;
        }

        set
        {
            bool isChange = !EqualityComparer<T>.Default.Equals(_Value, value);
            if (isChange)
            {
                _Value = value;
                OnChanged(value);
            }
        }
    }

    public Beep()
    {
        _Value = default(T);
    }
    public Beep(T value)
    {
        _Value = value;
    }

    public T Get()
    {
        return _Value;
    }

    public void Set(T value)
    {
        Value = value;
    }

    public static implicit operator T(Beep<T> x)
    {
        return x._Value;
    }

    T _Value = default(T);
}


[Serializable]
public abstract class BeepEventable<T>
{
    protected abstract UnityEvent<T> OnChangedEvent { get; }

    public T Value
    {
        get
        {
            return _Value;
        }

        set
        {
            bool isChange = !EqualityComparer<T>.Default.Equals(_Value, value);
            if (isChange)
            {
                _Value = value;
                OnChangedEvent.Invoke(value);
            }
        }
    }

    public T Get()
    {
        return _Value;
    }

    public void Set(T value)
    {
        Value = value;
    }

    public static implicit operator T(BeepEventable<T> x)
    {
        return x._Value;
    }

    public BeepEventable(T value)
    {
        _Value = value;
    }

    T _Value = default(T);
}

[Serializable]
public class BeepInt : BeepEventable<int>
{
    public UnityEventExtensions.Int OnValueChanged;

    protected override UnityEvent<int> OnChangedEvent
    {
        get { return OnValueChanged; }
    }

    public BeepInt() : base(Int32.MinValue) { }
}