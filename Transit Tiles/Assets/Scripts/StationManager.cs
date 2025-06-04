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
    [SerializeField] Board _board;

    public Board Board { get { return _board; } set { _board = value; } }

    [Header("Colors")]
    [SerializeField] public StationColor stationColor;

    [Header("Timers")]
    [SerializeField] private float stationTime;
    [SerializeField] private float travelTime;

    [Header("Booleans")]
    [SerializeField] public bool isTrainMoving = false;

    public static StationManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        stationColor = StationColor.RED;

        StartCoroutine(StationTimer());
    }

    public IEnumerator StationTimer()
    {
        yield return new WaitForSeconds(stationTime);

        Board.DisablePlatformTiles();
        isTrainMoving = true;
        Debug.Log("Train is now moving");

        StartCoroutine(TravelTimer());
    }

    public IEnumerator TravelTimer()
    {
        yield return new WaitForSeconds(travelTime);

        Board.EnablePlatformTiles();
        isTrainMoving = false;
        Debug.Log("Train has stopped");

        StartCoroutine(StationTimer());
    }

    public void CheckPassengerStation()
    {

    }
}
