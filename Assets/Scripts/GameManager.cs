using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;


public class GameManager : Singleton<GameManager>
{
    public Action<GameState, GameState> OnGameStateChange = delegate { };


    [Space(10)]
    [SerializeField]
    BaseMenu StartingMenu = null;
    [SerializeField]
    GameMenu GameMenu = null;
    [SerializeField]
    GameObject LevelObject = null;


    public enum GameState
    {
        None, Menu, Game
    };
    public GameState State { get; private set; }


    #region Behaviours
    new void Awake()
    {
        base.Awake();

        MenuManager.Instance.HideAllMenus();
        State = GameState.None;
    }

    void Start()
    {
        SetState(GameState.Menu);
    }
    #endregion

    public void SetState(GameState state)
    {
        var prevState = State;
        State = state;
        Debug.LogFormat("GameState: {0} -> {1}", prevState, State);
        OnGameStateChange(prevState, State);

        switch (state)
        {
            case GameState.Menu:
                MenuManager.Instance.ShowMenu(StartingMenu);
                LevelObject.SetActive(false);
                MatchManager.Instance.StopMatch();
                break;

            case GameState.Game:
                MenuManager.Instance.ShowMenu(GameMenu);
                LevelObject.SetActive(true);
                MatchManager.Instance.StartMatch();
                break;

            default:
                Debug.LogErrorFormat("Unsupported game state: {0}", state);
                break;
        }
    }

    public MobileUI SpawnMobileUI()
    {
        return GameMenu.SpawnMobileUI();
    }
}
