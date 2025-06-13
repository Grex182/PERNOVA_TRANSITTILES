using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public enum GameState
{
    GameTutorial,
    GameInit,
    GameStart,
    GameReset,
    GameEnded
}

public class GameManager : Singleton<GameManager>
{
    public GameState gameState;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        gameState = GameState.GameInit; // Initialize game
    }

    public void InitializeGame()
    {
        // Populate with variables that set to initial value
    }

    public void StartGame()
    {

    }

    public void EndGame()
    {

    }

    public void ResetGame()
    {

    }
}
