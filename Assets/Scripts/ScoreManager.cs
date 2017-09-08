using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Rand = UnityEngine.Random;


public class ScoreManager : Singleton<ScoreManager>
{
    public Action<int> OnHighScoreChanged = delegate { };


    public Beep<int> Score = new Beep<int>(0);


    [SerializeField]
    string HighscoreTag = "Highscore";


    #region Highscore
    public int HighScore
    {
        get { return PlayerPrefs.GetInt(HighscoreTag, 0); }
        set
        {
            Assert.IsTrue(HighScore < value, "New high score must be higher");

            PlayerPrefs.SetInt(HighscoreTag, value);
            PlayerPrefs.Save();
            OnHighScoreChanged(value);
        }
    }
    #endregion


    #region Behaviours
    void OnEnable()
    {
        Score.OnChanged += ScoreChanged;
        AsteroidSpawner.Instance.OnAsteroidDestroyed += AsteroidDestroyed;
    }

    void OnDisable()
    {
        Score.OnChanged -= ScoreChanged;
        if (AsteroidSpawner.Exists())
            AsteroidSpawner.Instance.OnAsteroidDestroyed -= AsteroidDestroyed;
    }
    #endregion


    #region Start/Stop
    public void StartMatch()
    {
        Score.Value = 0;
    }
    #endregion


    void AsteroidDestroyed(Asteroid asteroid)
    {
        Score.Value += asteroid.Parameters.Score;
    }

    void ScoreChanged(int score)
    {
        // Update highscore if needed
        if (score > HighScore)
            HighScore = score;
    }
}
