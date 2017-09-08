using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;
using TMPro;


public class GameOverPopup : BasePopup
{
    [SerializeField]
    TextMeshProUGUI HighScoreText = null;
    [SerializeField]
    TextMeshProUGUI ScoreText = null;


    public override void Show(object data = null)
    {
        base.Show(data);

        HighScoreText.text = String.Format("High Score: {0}", ScoreManager.Instance.HighScore);
        ScoreText.text = String.Format("Score: {0}", ScoreManager.Instance.Score.Value);
    }


    public void RestartPressed()
    {
        MenuManager.Instance.HidePopup();
        GameManager.Instance.SetState(GameManager.GameState.Game);
    }

    public void ExitPressed()
    {
        MenuManager.Instance.HidePopup();
        GameManager.Instance.SetState(GameManager.GameState.Menu);
    }
}
