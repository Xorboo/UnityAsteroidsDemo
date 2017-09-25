using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;
using Rand = UnityEngine.Random;


public class GameMenu : BaseMenu
{
    [SerializeField]
    TextMeshProUGUI ScoreText = null;
    [SerializeField]
    TextMeshProUGUI NewWaveText = null;
    [SerializeField, Range(0f, 10f)]
    float NewWaveTextDuration = 2f;

    [Space(10)]
    [SerializeField]
    GameObject HighScoreObject = null;
    [SerializeField]
    float HighscoreShowPause = 3f;

    [Space(10)]
    [SerializeField]
    Transform LifesRoot = null;
    [SerializeField]
    GameObject LifesPrefab = null;
    
    [Space(10)]
    [SerializeField]
    RectTransform RootUITransform = null;

    bool HighScoreWasShown;
    Coroutine WaveTextRoutine = null;


    #region Behaviours
    void OnEnable()
    {
        MatchManager.Instance.PlayerHealth.OnChanged += LifesChanged;
        ScoreManager.Instance.Score.OnChanged += ScoreChanged;
        ScoreManager.Instance.OnHighScoreChanged += HighScoreChanged;
        AsteroidSpawner.Instance.OnNewWaveSpawned += WaveSpawned;

        CreateLifes();
        LifesChanged(MatchManager.Instance.PlayerHealth);
        HighScoreWasShown = false;
        HighScoreObject.SetActive(false);
        ScoreChanged(ScoreManager.Instance.Score);
        HideNewWaveText();
    }

    void OnDisable()
    {
        if (MatchManager.Exists())
            MatchManager.Instance.PlayerHealth.OnChanged -= LifesChanged;
        if (ScoreManager.Exists())
            ScoreManager.Instance.Score.OnChanged -= ScoreChanged;
        if (ScoreManager.Exists())
            ScoreManager.Instance.OnHighScoreChanged -= HighScoreChanged;
        if (AsteroidSpawner.Exists())
            AsteroidSpawner.Instance.OnNewWaveSpawned -= WaveSpawned;
    }
    #endregion


    public RectTransform RootUI
    {
        get { return RootUITransform; }
    }


    #region Lifes
    void CreateLifes()
    {
        for(int i = LifesRoot.childCount, n = MatchManager.Instance.InitialHealth; i < n; i++)
            Instantiate(LifesPrefab, LifesRoot);
    }

    void LifesChanged(int lifes)
    {
        Debug.Log("Player lifes: " + lifes);
        Assert.IsTrue(lifes <= LifesRoot.childCount, "Player has more lifes than spawned in UI");

        for(int i = 0, n = LifesRoot.childCount; i < n; i++)
        {
            var life = LifesRoot.GetChild(i);
            life.gameObject.SetActive(i < lifes);
        }

        HideNewWaveText();
    }
    #endregion


    #region Score
    void ScoreChanged(int score)
    {
        ScoreText.text = score.ToString();
    }

    void HighScoreChanged(int highScore)
    {
        if (!HighScoreWasShown)
        {
            HighScoreWasShown = true;
            StartCoroutine(ShowHighScore());
        }
    }


    IEnumerator ShowHighScore()
    {
        HighScoreObject.SetActive(true);
        yield return new WaitForSeconds(HighscoreShowPause);
        HighScoreObject.SetActive(false);
    }
    #endregion


    #region New wave
    public void WaveSpawned(int waveIndex)
    {
        HideNewWaveText();

        NewWaveText.text = String.Format("Wave #{0}", waveIndex);
        WaveTextRoutine = StartCoroutine(ShowNewWaveText());
    }

    void HideNewWaveText()
    {
        NewWaveText.gameObject.SetActive(false);

        if (WaveTextRoutine != null)
        {
            StopCoroutine(WaveTextRoutine);
            WaveTextRoutine = null;
        }
    }

    IEnumerator ShowNewWaveText()
    {
        NewWaveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(NewWaveTextDuration);
        NewWaveText.gameObject.SetActive(false);

        WaveTextRoutine = null;
    }
    #endregion


    public override void EscapePressed()
    {
        base.EscapePressed();

        GameManager.Instance.SetState(GameManager.GameState.Menu);
    }
}
