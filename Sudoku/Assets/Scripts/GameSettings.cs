using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public enum EGameMode
    {
        NOT_SET,
        EASY,
        MEDIUM,
        HARD,
        VERY_HARD
    }

    public static GameSettings Instance;

    private void Awake()
    {
        _Paused = false;

        if(Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private EGameMode _GameMode;
    private bool _continuePreviusGame = false;
    private bool _exitAfterWon = false;
    private bool _Paused = false;

    public void SetExitAfterWon(bool set)
    {
        _exitAfterWon = set;
        _continuePreviusGame = false;
    }

    public bool GetExitAfterWon()
    {
        return _exitAfterWon;
    }

    public void SetContinuePreviousGame(bool continue_game)
    {
        _continuePreviusGame = continue_game;
    }

    public bool GetContinuePreviusGame()
    {
        return _continuePreviusGame;
    }

    public void SetPaused(bool paused) {_Paused = paused;}
    public bool GetPaused() {return _Paused;}
    void Start() 
    {
        _GameMode = EGameMode.NOT_SET;
        _continuePreviusGame = false;   
    }

    public void SetGameMode(EGameMode mode)
    {
        _GameMode = mode;
    }

    public void SetGameMode(string mode)
    {
        if(mode == "Easy") SetGameMode(EGameMode.EASY);
        else if(mode == "Medium") SetGameMode(EGameMode.MEDIUM);
        else if(mode == "Hard") SetGameMode(EGameMode.HARD);
        else if(mode == "VeryHard") SetGameMode(EGameMode.VERY_HARD);
        else SetGameMode(EGameMode.NOT_SET);
        
    }

    public string GetGameMode()
    {
        switch(_GameMode)
        {
            case EGameMode.EASY: return "Easy";
            case EGameMode.MEDIUM: return "Medium";
            case EGameMode.HARD: return "Hard";
            case EGameMode.VERY_HARD: return "VeryHard";
        }
        Debug.LogError("ERROR: Game Level Not Set");
        return " ";
    }
}
