using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


public class BaseMenu : MonoBehaviour
{
    #region Behaviours
    #endregion


    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void EscapePressed()
    {
        Debug.LogFormat("Escape pressed on menu '{0}'", gameObject.name);
    }
}
