using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rand = UnityEngine.Random;


public class MainMenu : BaseMenu
{
    [SerializeField]
    TextMeshProUGUI HighScore = null;


    #region Behaviours
    void OnEnable()
    {
        HighScore.text = "High Score: " + ScoreManager.Instance.HighScore.ToString();
    }
    #endregion


    public void StartClicked()
    {
        GameManager.Instance.SetState(GameManager.GameState.Game);
    }
}
