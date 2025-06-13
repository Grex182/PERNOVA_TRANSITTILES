using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StationColor
{
    Red,
    Pink,
    Orange,
    Yellow,
    Green,
    Blue,
    Violet
}

public class StationManager : Singleton<StationManager>
{
    [Header("Colors")]
    [SerializeField] public StationColor stationColor;

    [Header("Timers")]
    [SerializeField] private float stationTime;
    [SerializeField] private float travelTime;

    [Header("Booleans")]
    [SerializeField] public bool isTrainMoving = false;
    [SerializeField] public bool hasGameStarted = true;
    [SerializeField] public bool isGoingBackToStartingStation = false;

    private int currentStationIndex = 0;
    private int direction = 1; // 1 = forward, -1 = backward

    private void Start()
    {
        GameManager.instance.StationManager = this;

        stationColor = StationColor.Red;

        StartCoroutine(StationTimer());
    }

    public IEnumerator StationTimer()
    {
        yield return new WaitForSeconds(stationTime);

        GameManager.instance.Board.DisablePlatformTiles();
        isTrainMoving = true;

        if (hasGameStarted)
        {
            hasGameStarted = false;
        }

        Debug.Log("Train is now moving");

        StartCoroutine(TravelTimer());
    }

    public IEnumerator TravelTimer()
    {
        yield return new WaitForSeconds(travelTime);

        GameManager.instance.Board.EnablePlatformTiles();
        isTrainMoving = false;
        Debug.Log("Train has stopped");

        UpdateStationColor();

        StartCoroutine(StationTimer());
    }

    private void UpdateStationColor()
    {
        // Get total number of station colors
        int totalStations = System.Enum.GetValues(typeof(StationColor)).Length;

        // Update index
        currentStationIndex += direction;

        // If we hit the bounds, reverse direction
        if (currentStationIndex >= totalStations)
        {
            currentStationIndex = totalStations - 2; // go one step before last
            direction = -1;

            isGoingBackToStartingStation = true;
        }
        else if (currentStationIndex < 0)
        {
            currentStationIndex = 1; // go one step after first
            direction = 1;

            isGoingBackToStartingStation = false;
        }

        // Set new station color
        stationColor = (StationColor)currentStationIndex;

        Debug.Log("The Train has arrived at: " + stationColor + "Station.");
    }
}
