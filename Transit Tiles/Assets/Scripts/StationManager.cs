using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StationColor
{
    RED,
    PINK,
    ORANGE,
    YELLOW,
    GREEN,
    BLUE,
    VIOLET
}

public class StationManager : Singleton<StationManager>
{
    [SerializeField] public StationColor stationColor;

    public static StationManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        stationColor = StationColor.RED;
    }

    public void SendMessageTest()
    {
        Debug.Log("Looks like StationManager is working.");
    }
}
