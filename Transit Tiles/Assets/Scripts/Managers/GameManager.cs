using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] Board _board;

    public Board Board { get { return _board; } set { _board = value; } }

    [SerializeField] StationManager _stationManager;

    public StationManager StationManager { get { return _stationManager; } set { _stationManager = value; } }

    [SerializeField] PublicRatingManager _publicRatingManager;

    public PublicRatingManager PublicRatingManager { get { return _publicRatingManager; } set { _publicRatingManager = value; } }

    [SerializeField] ScoreManager _scoreManager;

    public ScoreManager ScoreManager { get { return _scoreManager; } set { _scoreManager = value; } }

    [SerializeField] StageSpawner _stageSpawner;

    public StageSpawner StageSpawner { get { return _stageSpawner; } set { _stageSpawner = value; } }

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }
}
