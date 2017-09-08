using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


public class BasePopup : MonoBehaviour
{
    #region Behaviours
    #endregion


    public virtual void Show(object data = null)
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void EscapePressed()
    {
        Debug.LogFormat("Escape pressed on popup '{0}'", gameObject.name);
    }
}
