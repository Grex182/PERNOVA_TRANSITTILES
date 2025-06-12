using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [Header("Player Score")]
    [SerializeField] public int currentScore = 0;
    [SerializeField] public int baseScoreValue = 100;

    private void Start()
    {
        GameManager.instance.ScoreManager = this;
    }

    public void AddScore()
    {
        //Happy Standard: 10 points | Happy Priority: 50 points | Landing the station: 100 points (When player lands on a station, they get 100 points, the passengers are just plus points)
        currentScore += 100;

        Debug.Log("Current Score: " + currentScore);
        //Add the update text line under the score line
    }
}
