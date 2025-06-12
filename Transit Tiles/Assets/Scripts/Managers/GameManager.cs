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

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
}
